namespace Tgs.GameInterface.HabaneroService.Core.Models;

public record QueryPlayerResult(
    bool IsPlayerExist,
    decimal Balance);
