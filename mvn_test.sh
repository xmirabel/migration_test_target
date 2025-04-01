#!/bin/bash

# Couleurs pour les messages
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${YELLOW}=== Ex�cution des tests avec Maven ===${NC}"

# Ex�cuter les tests avec Maven
mvn test

# R�cup�rer le code de retour
result=$?

# Afficher le r�sultat
if [ $result -eq 0 ]; then
    echo -e "\n${GREEN}SUCC�S: Tous les tests ont r�ussi.${NC}"
else
    echo -e "\n${RED}�CHEC: Certains tests ont �chou�.${NC}"
fi

exit $result
