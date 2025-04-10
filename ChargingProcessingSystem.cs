using System;
using System.IO;
using System.Collections.Generic;

namespace ChargingProcessingSystem
{
    /// <summary>
    /// APOST Process - Converted from COBOL program IBMTEST3
    /// This program handles equipment movement processing and reclaim operations
    /// </summary>
    public class EquipmentProcessingService
    {
        #region Fields and Properties

        // Saved values for processing
        private string _savedEquipmentInitNumber;
        private string _savedReclaimCode;
        private int _savedTripControlNumber;

        // System glossary values - converted from COPY SY101CBGL
        private class GeneralGlossary
        {
            public const string Yes = "Y";
            public const string No = "N";

            public const byte IndicatorOn = 1;
            public const byte IndicatorOff = 0;

            public class ScreenStatus
            {
                public const string F3 = "F3 ";
                public const string F12 = "F12";
            }

            public class LogicalViews
            {
                public const string L01 = "L01";
                public const string L02 = "L02";
                public const string L03 = "L03";
                public const string L04 = "L04";
            }
        }

        // Database I/O glossary - converted from COPY SY102CBGL
        private class DbIoGlossary
        {
            public class IoCommands
            {
                public const string Read = "R  ";
                public const string ReadFirst = "RF ";
                public const string ReadLast = "RL ";
                public const string ReadNext = "RN ";
                public const string ReadPrevious = "RP ";
                public const string Add = "A  ";
                public const string Update = "U  ";
                public const string Delete = "D  ";
                public const string OpenIO = "OIO";
                public const string OpenInput = "OI ";
                public const string Close = "C  ";
                public const string ReadSeries = "RS ";
                public const string ReadLocked = "RK ";
                public const string ReadNextLocked = "RNK";
            }

            public class FileStatus
            {
                public const string NoIO = "  ";
                public const string StatusOk = "00";
                public const string EndOfFile = "10";
                public const string NotFound = "23";
            }

            public class ReturnStatus
            {
                public const int Normal = 0;
                public const int BadCommand = 1;
                public const int BadClose = 2;
                public const int BadSegmentId = 3;
                public const int IoFailure = 4;
                public const int FileNotOpen = 5;
                public const int NoIO = 99;
            }

            public class SwitchValues
            {
                public const string Open = "O";
                public const string Close = "C";
            }
        }

        // Data structures representing the various records and system options
        private SystemOptions _systemOptions;
        private CycleHeader _cycleHeader;
        private CycleHeader _prevCycleHeader;
        private CycleHeader _lastCycleHeader;
        private CycleHeader _wantCycleHeader;
        private CycleHeader _cycleHeaderShadow;
        private EquipmentMovementHistory _equipMovementHistory;
        private EquipmentMovementEvent _equipMovementEvent;
        private EquipmentMovementEvent _prevEquipMovementEvent;
        private EquipmentMovementEvent _beforePrevEquipMovementEvent;
        private ReclaimMaster _reclaimMaster;
        private TripInfo _tripInfo;
        private ProcessError _processError;
        private ProcessError _prevProcessError;

        // Control flags and status indicators
        private string _fileStatus;
        private string _ioCommand;
        private bool _isEndOfFile;
        private string _currentSection;
        private string _abortComment;
        private bool _utchprclm3Called;
        private string _utchp02Director;
        private int _d94Index;
        private string _d99GoNogoIndicator;
        private string _d93ReclaimCodeExt;
        private int _d95TripControlNumber;
        private bool _d93MaxReclaim;

        // External service interfaces
        private IIoService _ioService;
        private IUtilityService _utilityService;
        private ILogService _logService;

        #endregion

        #region Constructor

        public EquipmentProcessingService(
            IIoService ioService,
            IUtilityService utilityService,
            ILogService logService)
        {
            _ioService = ioService;
            _utilityService = utilityService;
            _logService = logService;

            // Initialize status fields
            _fileStatus = DbIoGlossary.FileStatus.NoIO;
            _utchprclm3Called = false;
            _utchp02Director = string.Empty;
        }

        #endregion

        #region Main Processing Methods

        /// <summary>
        /// Main entry point for the equipment processing service
        /// </summary>
        /// <param name="phase">Processing phase indicator</param>
        public void ProcessEquipment(string phase)
        {
            try
            {
                // Initialize system options
                _systemOptions = _ioService.GetSystemOptions();

                // Check if program was aborted previously
                if (!string.IsNullOrEmpty(_systemOptions.AbortedInProgram))
                {
                    return; // Exit if aborted
                }

                // Determine if we need to retrieve historical or current data
                if (!string.IsNullOrEmpty(_systemOptions.ModsEffectiveMonth) &&
                    _systemOptions.ServiceMonth.CompareTo(_systemOptions.ModsEffectiveMonth) < 0)
                {
                    _utilityService.SetRetrieveHistorical();
                }
                else
                {
                    _utilityService.SetRetrieveCurrent();
                }

                // Initialize processing
                _utilityService.ClearErrors();
                _utilityService.InitializeProcessing(_systemOptions);

                // Perform main processing steps
                PerformHousekeeping();

                // Main processing loop
                while (!_isEndOfFile)
                {
                    ProcessingLoop();
                }

                CloseFiles();

                // Update system options
                _ioService.UpdateSystemOptions(_systemOptions);

                // Conditional processing based on phase
                if (phase == "1")
                {
                    if (_systemOptions.IsTolParticipant)
                    {
                        if (_systemOptions.IsVoluntaryTrip || _systemOptions.IsEstimateTrip)
                        {
                            _utilityService.ProcessReclaimSecondary();
                        }
                    }

                    if (_d93MaxReclaim)
                    {
                        _utilityService.ProcessReclaimFifth();
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError($"Error in ProcessEquipment: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Performs initial setup and validation before main processing
        /// </summary>
        private void PerformHousekeeping()
        {
            _currentSection = "Housekeeping";

            _utchprclm3Called = false;
            _utchp02Director = string.Empty;

            // Validate processing phase
            if (_systemOptions.Phase != "1" && _systemOptions.Phase != "2")
            {
                _abortComment = "Missing Phase";
                AbortProcessing();
                return;
            }

            // Initialize data structures
            InitializeDataStructures();

            // Open required files
            _fileStatus = DbIoGlossary.FileStatus.NoIO;
            OpenFiles();

            // Initialize process error tracking
            _processError = new ProcessError();
            _prevProcessError = new ProcessError();

            // Set user road and other key fields
            _processError.UserRoad = _systemOptions.UserRoad;
            _processError.BeginCycleControlNumber = _systemOptions.BeginCycleControlNumber;
            _processError.ServiceMonth = _systemOptions.ServiceMonth;
            _processError.DisplayServiceMonth = _systemOptions.DisplayServiceMonth;

            // Phase 1 specific processing
            if (_systemOptions.Phase == "1")
            {
                // Find maximum reclaim code
                _reclaimMaster = new ReclaimMaster();
                _reclaimMaster.UserRoad = _systemOptions.UserRoad;

                _ioCommand = DbIoGlossary.IoCommands.ReadNext;

                // Read until EOF or until we find the MAX reclaim code
                while (!_isEndOfFile && _reclaimMaster.ReclaimCodeExt != "MAX ")
                {
                    PerformDatabaseIO(ReclaimMasterFile);
                }

                if (_isEndOfFile)
                {
                    _fileStatus = "  ";
                }
                else
                {
                    _d93ReclaimCodeExt = _reclaimMaster.ReclaimCodeExt;
                }
            }

            // Continue with common processing
            _reclaimMaster = new ReclaimMaster();
            _reclaimMaster.UserRoad = _systemOptions.UserRoad;

            _ioCommand = DbIoGlossary.IoCommands.ReadNext;
            PerformDatabaseIO(ReclaimMasterFile);

            if (_isEndOfFile)
            {
                _d99GoNogoIndicator = "N";
            }

            // Set up cycle header and trip info
            _cycleHeader.UserRoad = _systemOptions.UserRoad;
            _tripInfo.UserRoad = _systemOptions.UserRoad;

            _ioCommand = DbIoGlossary.IoCommands.ReadNext;
            PerformDatabaseIO(CycleHeaderFile);

            // Initialize text processing utility
            _utilityService.InitializeTextProcessing(
                _cycleHeader,
                _reclaimMaster,
                _systemOptions);
        }

        /// <summary>
        /// Main processing loop - processes each record
        /// </summary>
        private void ProcessingLoop()
        {
            // Note: This method would contain the implementation of the 2000-PROCESSING-LOOP section
            // Since it's not provided in the original COBOL snippet, I'm including a placeholder

            // Read next record
            _ioCommand = DbIoGlossary.IoCommands.ReadNext;
            PerformDatabaseIO(CycleHeaderFile);

            // Process the record according to business rules
            // ...
        }

        /// <summary>
        /// Retrieves reclaim information for the current cycle
        /// </summary>
        private void GetReclaimInfo()
        {
            _currentSection = "GetReclaimInfo";

            // Initialize reclaim master
            _reclaimMaster = new ReclaimMaster();
            _reclaimMaster.UserRoad = _systemOptions.UserRoad;
            _reclaimMaster.ReclaimCode = _cycleHeader.ReclaimCode;
            _reclaimMaster.ReclaimCodeExt = _cycleHeader.ReclaimCodeExt;

            // Read reclaim master record
            ReadReclaimMaster();

            // Validate the record
            if (!_fileStatus.Equals(DbIoGlossary.FileStatus.StatusOk))
            {
                _reclaimMaster = new ReclaimMaster();
                _fileStatus = string.Empty;
                return;
            }

            // Check if we've gone past the desired record
            if (_reclaimMaster.ReclaimCode.CompareTo(_cycleHeader.ReclaimCode) > 0)
            {
                return;
            }

            if (_reclaimMaster.ReclaimCode.Equals(_cycleHeader.ReclaimCode) &&
                _reclaimMaster.ReclaimCodeExt.CompareTo(_cycleHeader.ReclaimCodeExt) > 0)
            {
                return;
            }

            // Check effective/expiration dates
            if (_reclaimMaster.EffectiveMonth.CompareTo(_systemOptions.ServiceMonth) > 0)
            {
                _reclaimMaster = new ReclaimMaster();
                return;
            }

            if (_reclaimMaster.ExpireMonth.CompareTo(_systemOptions.ServiceMonth) < 0)
            {
                ReadReclaimMaster();
            }
        }

        /// <summary>
        /// Helper method to read the reclaim master record
        /// </summary>
        private void ReadReclaimMaster()
        {
            _ioCommand = DbIoGlossary.IoCommands.ReadNext;
            PerformDatabaseIO(ReclaimMasterFile);
        }

        /// <summary>
        /// Detects Time-On-Line (TOL) failures in the equipment movement
        /// </summary>
        private void DetectTolFailure()
        {
            _currentSection = "DetectTolFailure";

            // Only process for voluntary trips or estimate trips
            if (!_systemOptions.IsVoluntaryTrip && !_systemOptions.IsEstimateTrip)
            {
                return;
            }

            // Initialize equipment movement history
            _equipMovementHistory = new EquipmentMovementHistory();
            _equipMovementHistory.UserRoad = _systemOptions.UserRoad;
            _equipMovementHistory.TripControlNumber = _cycleHeader.TripControlNumber;
            _d95TripControlNumber = _cycleHeader.TripControlNumber;

            _fileStatus = string.Empty;
            _ioCommand = DbIoGlossary.IoCommands.ReadNext;
            PerformDatabaseIO(EquipmentMovementHistoryFile);

            // Process based on reclaim code and reload status
            if (!string.IsNullOrEmpty(_cycleHeader.ReclaimCodeExt) && _cycleHeader.IsReload)
            {
                // Loop until we find a delivery with TOL Rule 5 receivable
                while (!_isEndOfFile &&
                       _equipMovementHistory.TripControlNumber == _d95TripControlNumber &&
                       !(_equipMovementHistory.IsTolRule5Receivable && _equipMovementHistory.IsDelivery))
                {
                    PerformDatabaseIO(EquipmentMovementHistoryFile);
                }
            }
            else
            {
                // Loop until we find any TOL Rule 5 receivable
                while (!_isEndOfFile &&
                       _equipMovementHistory.TripControlNumber == _d95TripControlNumber &&
                       !_equipMovementHistory.IsTolRule5Receivable)
                {
                    PerformDatabaseIO(EquipmentMovementHistoryFile);
                }
            }

            // Check for failure conditions
            if (_isEndOfFile || _equipMovementHistory.TripControlNumber != _d95TripControlNumber)
            {
                HandleTolFailure();
            }
        }

        /// <summary>
        /// Handles the TOL failure scenario
        /// </summary>
        private void HandleTolFailure()
        {
            // This would implement the 3400-50-FAILED section
            // Since it's not provided in the original COBOL snippet, I'm including a placeholder

            _logService.LogWarning($"TOL failure detected for trip {_d95TripControlNumber}");

            // Process the failure according to business rules
            // ...
        }

        #endregion

        #region Database Operations

        /// <summary>
        /// Performs database I/O operations based on the current command
        /// </summary>
        /// <param name="fileType">The type of file to perform operations on</param>
        private void PerformDatabaseIO(string fileType)
        {
            try
            {
                switch (fileType)
                {
                    case CycleHeaderFile:
                        PerformCycleHeaderIO();
                        break;
                    case ReclaimMasterFile:
                        PerformReclaimMasterIO();
                        break;
                    case EquipmentMovementHistoryFile:
                        PerformEquipmentMovementHistoryIO();
                        break;
                    default:
                        throw new ArgumentException($"Unknown file type: {fileType}");
                }
            }
            catch (Exception ex)
            {
                _logService.LogError($"Database I/O error in {_currentSection}: {ex.Message}");
                _fileStatus = DbIoGlossary.FileStatus.NotFound;
                _isEndOfFile = true;
            }
        }

        /// <summary>
        /// Performs I/O operations on the Cycle Header file
        /// </summary>
        private void PerformCycleHeaderIO()
        {
            switch (_ioCommand)
            {
                case DbIoGlossary.IoCommands.Read:
                    _cycleHeader = _ioService.ReadCycleHeader(_cycleHeader.UserRoad, _cycleHeader.CycleControlNumber);
                    _isEndOfFile = (_cycleHeader == null);
                    _fileStatus = _isEndOfFile ? DbIoGlossary.FileStatus.EndOfFile : DbIoGlossary.FileStatus.StatusOk;
                    break;

                case DbIoGlossary.IoCommands.ReadNext:
                    _cycleHeader = _ioService.ReadNextCycleHeader(_cycleHeader.UserRoad);
                    _isEndOfFile = (_cycleHeader == null);
                    _fileStatus = _isEndOfFile ? DbIoGlossary.FileStatus.EndOfFile : DbIoGlossary.FileStatus.StatusOk;
                    break;

                // Additional I/O commands would be implemented here

                default:
                    _logService.LogWarning($"Unsupported I/O command for Cycle Header: {_ioCommand}");
                    _fileStatus = DbIoGlossary.FileStatus.NotFound;
                    break;
            }
        }

        /// <summary>
        /// Performs I/O operations on the Reclaim Master file
        /// </summary>
        private void PerformReclaimMasterIO()
        {
            switch (_ioCommand)
            {
                case DbIoGlossary.IoCommands.Read:
                    _reclaimMaster = _ioService.ReadReclaimMaster(
                        _reclaimMaster.UserRoad,
                        _reclaimMaster.ReclaimCode,
                        _reclaimMaster.ReclaimCodeExt);
                    _isEndOfFile = (_reclaimMaster == null);
                    _fileStatus = _isEndOfFile ? DbIoGlossary.FileStatus.EndOfFile : DbIoGlossary.FileStatus.StatusOk;
                    break;

                case DbIoGlossary.IoCommands.ReadNext:
                    _reclaimMaster = _ioService.ReadNextReclaimMaster(_reclaimMaster.UserRoad);
                    _isEndOfFile = (_reclaimMaster == null);
                    _fileStatus = _isEndOfFile ? DbIoGlossary.FileStatus.EndOfFile : DbIoGlossary.FileStatus.StatusOk;
                    break;

                // Additional I/O commands would be implemented here

                default:
                    _logService.LogWarning($"Unsupported I/O command for Reclaim Master: {_ioCommand}");
                    _fileStatus = DbIoGlossary.FileStatus.NotFound;
                    break;
            }
        }

        /// <summary>
        /// Performs I/O operations on the Equipment Movement History file
        /// </summary>
        private void PerformEquipmentMovementHistoryIO()
        {
            switch (_ioCommand)
            {
                case DbIoGlossary.IoCommands.Read:
                    _equipMovementHistory = _ioService.ReadEquipmentMovementHistory(
                        _equipMovementHistory.UserRoad,
                        _equipMovementHistory.TripControlNumber);
                    _isEndOfFile = (_equipMovementHistory == null);
                    _fileStatus = _isEndOfFile ? DbIoGlossary.FileStatus.EndOfFile : DbIoGlossary.FileStatus.StatusOk;
                    break;

                case DbIoGlossary.IoCommands.ReadNext:
                    _equipMovementHistory = _ioService.ReadNextEquipmentMovementHistory(
                        _equipMovementHistory.UserRoad,
                        _equipMovementHistory.TripControlNumber);
                    _isEndOfFile = (_equipMovementHistory == null);
                    _fileStatus = _isEndOfFile ? DbIoGlossary.FileStatus.EndOfFile : DbIoGlossary.FileStatus.StatusOk;
                    break;

                // Additional I/O commands would be implemented here

                default:
                    _logService.LogWarning($"Unsupported I/O command for Equipment Movement History: {_ioCommand}");
                    _fileStatus = DbIoGlossary.FileStatus.NotFound;
                    break;
            }
        }

        /// <summary>
        /// Opens all required files for processing
        /// </summary>
        private void OpenFiles()
        {
            try
            {
                _logService.LogInfo("Opening files for processing");

                // Open Cycle Header file
                _ioService.OpenCycleHeaderFile(_systemOptions.UserRoad);

                // Open Reclaim Master file
                _ioService.OpenReclaimMasterFile(_systemOptions.UserRoad);

                // Open Equipment Movement History file
                _ioService.OpenEquipmentMovementHistoryFile(_systemOptions.UserRoad);

                // Additional files would be opened here

                _fileStatus = DbIoGlossary.FileStatus.StatusOk;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Error opening files: {ex.Message}");
                _fileStatus = DbIoGlossary.FileStatus.NotFound;
                AbortProcessing();
            }
        }

        /// <summary>
        /// Closes all open files
        /// </summary>
        private void CloseFiles()
        {
            try
            {
                _logService.LogInfo("Closing files");

                // Close Cycle Header file
                _ioService.CloseCycleHeaderFile();

                // Close Reclaim Master file
                _ioService.CloseReclaimMasterFile();

                // Close Equipment Movement History file
                _ioService.CloseEquipmentMovementHistoryFile();

                // Additional files would be closed here

                _fileStatus = DbIoGlossary.FileStatus.StatusOk;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Error closing files: {ex.Message}");
                _fileStatus = DbIoGlossary.FileStatus.NotFound;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Initializes all data structures used in the program
        /// </summary>
        private void InitializeDataStructures()
        {
            _cycleHeader = new CycleHeader();
            _equipMovementHistory = new EquipmentMovementHistory();
            _equipMovementEvent = new EquipmentMovementEvent();
            _prevEquipMovementEvent = new EquipmentMovementEvent();
            _beforePrevEquipMovementEvent = new EquipmentMovementEvent();
            _prevCycleHeader = new CycleHeader();
            _lastCycleHeader = new CycleHeader();
            _wantCycleHeader = new CycleHeader();
            _cycleHeaderShadow = new CycleHeader();
            _d94Index = 0;
            _tripInfo = new TripInfo();
        }

        /// <summary>
        /// Aborts processing with an error message
        /// </summary>
        private void AbortProcessing()
        {
            _logService.LogError($"Processing aborted in {_currentSection}: {_abortComment}");
            _systemOptions.AbortedInProgram = "IBMTEST3";
            _systemOptions.AbortReason = _abortComment;

            // Close any open files
            CloseFiles();

            throw new ApplicationException($"Processing aborted: {_abortComment}");
        }

        #endregion

        #region Constants

        // File type constants
        private const string CycleHeaderFile = "CycleHeader";
        private const string ReclaimMasterFile = "ReclaimMaster";
        private const string EquipmentMovementHistoryFile = "EquipmentMovementHistory";

        #endregion
    }

    #region Data Models

    /// <summary>
    /// Represents system options and configuration
    /// </summary>
    public class SystemOptions
    {
        public string UserRoad { get; set; }
        public string BeginCycleControlNumber { get; set; }
        public string ServiceMonth { get; set; }
        public string DisplayServiceMonth { get; set; }
        public string ModsEffectiveMonth { get; set; }
        public string AbortedInProgram { get; set; }
        public string AbortReason { get; set; }
        public bool IsTolParticipant { get; set; }
        public bool IsVoluntaryTrip { get; set; }
        public bool IsEstimateTrip { get; set; }
        public string Phase { get; set; }
    }

    /// <summary>
    /// Represents a cycle header record
    /// </summary>
    public class CycleHeader
    {
        public string UserRoad { get; set; }
        public string CycleControlNumber { get; set; }
        public int TripControlNumber { get; set; }
        public string ReclaimCode { get; set; }
        public string ReclaimCodeExt { get; set; }
        public bool IsReload { get; set; }
    }

    /// <summary>
    /// Represents an equipment movement history record
    /// </summary>
    public class EquipmentMovementHistory
    {
        public string UserRoad { get; set; }
        public int TripControlNumber { get; set; }
        public bool IsTolRule5Receivable { get; set; }
        public bool IsDelivery { get; set; }
    }

    /// <summary>
    /// Represents an equipment movement event
    /// </summary>
    public class EquipmentMovementEvent
    {
        public string UserRoad { get; set; }
        public int EventId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventType { get; set; }
        public string LocationCode { get; set; }
    }

    /// <summary>
    /// Represents a reclaim master record
    /// </summary>
    public class ReclaimMaster
    {
        public string UserRoad { get; set; }
        public string ReclaimCode { get; set; }
        public string ReclaimCodeExt { get; set; }
        public string EffectiveMonth { get; set; }
        public string ExpireMonth { get; set; }
        public decimal ReclaimRate { get; set; }
        public string ReclaimDescription { get; set; }
    }

    /// <summary>
    /// Represents trip information
    /// </summary>
    public class TripInfo
    {
        public string UserRoad { get; set; }
        public int TripControlNumber { get; set; }
        public DateTime TripDate { get; set; }
        public string OriginLocation { get; set; }
        public string DestinationLocation { get; set; }
    }

    /// <summary>
    /// Represents process error information
    /// </summary>
    public class ProcessError
    {
        public string UserRoad { get; set; }
        public string BeginCycleControlNumber { get; set; }
        public string ServiceMonth { get; set; }
        public string DisplayServiceMonth { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }

    #endregion

    #region Service Interfaces

    /// <summary>
    /// Interface for I/O operations
    /// </summary>
    public interface IIoService
    {
        SystemOptions GetSystemOptions();
        void UpdateSystemOptions(SystemOptions options);

        void OpenCycleHeaderFile(string userRoad);
        void OpenReclaimMasterFile(string userRoad);
        void OpenEquipmentMovementHistoryFile(string userRoad);

        void CloseCycleHeaderFile();
        void CloseReclaimMasterFile();
        void CloseEquipmentMovementHistoryFile();

        CycleHeader ReadCycleHeader(string userRoad, string cycleControlNumber);
        CycleHeader ReadNextCycleHeader(string userRoad);

        ReclaimMaster ReadReclaimMaster(string userRoad, string reclaimCode, string reclaimCodeExt);
        ReclaimMaster ReadNextReclaimMaster(string userRoad);

        EquipmentMovementHistory ReadEquipmentMovementHistory(string userRoad, int tripControlNumber);
        EquipmentMovementHistory ReadNextEquipmentMovementHistory(string userRoad, int tripControlNumber);
    }

    /// <summary>
    /// Interface for utility operations
    /// </summary>
    public interface IUtilityService
    {
        void SetRetrieveHistorical();
        void SetRetrieveCurrent();
        void ClearErrors();
        void InitializeProcessing(SystemOptions options);
        void ProcessReclaimSecondary();
        void ProcessReclaimFifth();
        void InitializeTextProcessing(CycleHeader header, ReclaimMaster master, SystemOptions options);
    }

    /// <summary>
    /// Interface for logging operations
    /// </summary>
    public interface ILogService
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
    }

    #endregion
}

