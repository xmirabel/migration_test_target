#!/bin/bash

# Couleurs pour les messages
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${YELLOW}=== Exécution des tests avec Maven ===${NC}"

# Exécuter les tests avec Maven
mvn test

# Récupérer le code de retour
result=$?

# Afficher le résultat
if [ $result -eq 0 ]; then
    echo -e "\n${GREEN}SUCCÈS: Tous les tests ont réussi.${NC}"
else
    echo -e "\n${RED}ÉCHEC: Certains tests ont échoué.${NC}"
fi

exit $result
