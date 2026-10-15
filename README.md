# GyBlood (application mobile .NET MAUI)

Application Android unique pour les cinq rôles (patient, donneur, banque de sang / hôpital, sponsor, administrateur).
Elle consomme l'API REST FastAPI (Python, dossier `gyblood-api`) décrite dans le document « GyBlood — Cahier des charges et conception » (sections 5 à 8).

## État du squelette

**Ce qui est écrit** : solution, configuration, injection de dépendances, clients Refit pour tous les endpoints du MVP,
authentification (email et Google), renouvellement du jeton, sélection du Shell par rôle, écrans de connexion,
d'inscription et de compte en attente, service de position, signal de présence du donneur, compatibilité ABO/Rh, tests.

**Ce qui est à écrire** : les 28 autres écrans sont des coquilles (page et ViewModel qui s'affichent et indiquent la
user story à réaliser). La carte affiche le fond OpenStreetMap sans marqueurs. Les notifications push (Firebase) sont à brancher.

**Non compilé** : ce squelette a été généré sans SDK .NET. Lancez un premier `dotnet build` : de petites corrections
(versions de paquets, API de Mapsui) sont possibles.

## Prérequis

- SDK .NET 10 et workload MAUI Android : `dotnet workload install maui-android`
- Android SDK et un émulateur (ou un téléphone en mode développeur)
- Backend FastAPI lancé sur le port 8080 (`uvicorn app.main:app --port 8080`, voir `gyblood-api/README.md`) (voir `Core/Constants/AppConstants.cs`)

## Démarrage

```bash
dotnet restore
dotnet build GyBlood.Mobile -f net10.0-android
dotnet run --project GyBlood.Mobile -f net10.0-android   # avec un émulateur ouvert
dotnet test GyBlood.Mobile.Tests
```

L'émulateur Android voit le PC sous l'adresse `10.0.2.2`. Sur un vrai téléphone, remplacez `ApiBaseUrl` par l'adresse IP du PC
ou par l'URL déployée.

Une fois le premier restore réussi, **figez les versions** des paquets marqués `*` dans `GyBlood.Mobile.csproj`.

## Organisation

| Dossier | Rôle |
| --- | --- |
| `Core/` | Constantes (routes d'API et de navigation), énumérations, helpers purs, extensions et enregistrements DI |
| `Models/` | Requêtes et réponses (DTO) de l'API |
| `Services/` | Clients Refit, authentification, position, présence, navigation, boîtes de dialogue |
| `ViewModels/` | Un dossier par rôle, même structure que `Views/` |
| `Views/` | Pages XAML, Shells par rôle (`Views/Shells/`), contrôles (`Views/Controls/`) |
| `Converters/`, `Behaviors/` | Convertisseurs et validations de saisie |

Écarts avec l'arborescence du document : ajout de `StartupPage` (restaure la session au lancement), de `IMeApi`
(compte connecté, appareil, présence), de `SponsorProfileViewModel`, de `WebAuthenticationCallbackActivity` (retour Google),
de `RouteRegistration` (pages empilées) et de `Core/GlobalUsings.cs`.

## Ajouter un écran

1. Créer `XxxViewModel` (hérite de `BaseViewModel`) et `XxxPage.xaml(.cs)` dans le dossier du rôle.
2. L'enregistrer dans `Core/Extensions/ServiceCollectionExtensions.cs` (`AddViewModelsAndPages`).
3. Onglet : l'ajouter au Shell du rôle. Page empilée : ajouter la route dans `AppRoutes` et `RouteRegistration`,
   puis naviguer avec `INavigationService.GoToAsync(AppRoutes.Xxx, new Dictionary<string, object> { ["id"] = 12 })`.

## Contrat à valider avec le backend

- **Identifiants** : entiers (`long`), comme dans le backend FastAPI.
- **Énumérations** : envoyées et attendues en `MAJUSCULES_AVEC_TIRET_BAS` (`NO_SHOW`, `MTN_MOMO`...), y compris dans les paramètres d'URL.
  Les statuts français du cahier des charges (`DEMANDÉ`, `EN_ATTENTE`) correspondent à `REQUESTED`, `PENDING`.
- **Pages** : listes paginées au format Spring Data (`content`, `totalElements`, `totalPages`, `number`, `size`).
- **Erreurs** : `{ "code": "...", "message": "...", "details": {} }`. Le champ `message` est affiché tel quel à l'utilisateur.
- **Connexion Google (PKCE)** : l'application génère un `code_verifier`, ouvre `GoogleAuthUrl?code_challenge=...&role=...` ; après consentement
  le backend redirige vers `gyblood://auth?code=...` (ou `?error=account_exists|google_email_unverified|access_denied`), puis
  l'application appelle `POST /auth/google/exchange {code, codeVerifier}` pour obtenir les jetons. Aucun jeton ne passe par l'URL.
- **Mots de passe et clés** : l'application ne contient aucune clé d'API (Google, agrégateur de paiement) : elles restent côté serveur.
  `google-services.json` (Firebase) est ignoré par Git.

## Planning

Voir la section 8 du document : semaine 1 authentification, semaine 2 donneur et notifications, semaine 3 patient, carte et rendez-vous,
semaine 4 paiement, campagnes et sponsors.
