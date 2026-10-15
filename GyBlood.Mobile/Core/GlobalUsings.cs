// Alias de l'identifiant des entités. Le backend FastAPI utilise des entiers (long) pour les identifiants.
// Si UUID : remplacer "long" par "System.Guid" ici, c'est le seul endroit à changer.
global using EntityId = long;

global using GyBlood.Mobile.Core.Enums;
global using GyBlood.Mobile.Models.Requests;
global using GyBlood.Mobile.Models.Responses;
