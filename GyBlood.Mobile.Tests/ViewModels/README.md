# Tests de ViewModels

Les ViewModels dépendent de MAUI (Shell, SecureStorage, etc.) et ne peuvent pas être compilés dans ce projet `net10.0`.

Pour les tester, deux options :

1. Isoler la logique dans des classes sans dépendance MAUI (comme `InputValidator` ou `BloodCompatibility`) et les tester ici.
2. Créer un second projet de test qui cible `net10.0-android` et référence `GyBlood.Mobile` (exécution sur émulateur).

Les faux services du dossier `Fakes/` servent à brancher l'interface sur des données de démonstration sans backend.
