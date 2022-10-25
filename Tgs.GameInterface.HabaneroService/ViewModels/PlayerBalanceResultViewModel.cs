namespace Tgs.GameInterface.HabaneroService.ViewModels;

public record PlayerBalanceResultViewModel(
    bool IsPlayerExist,
    decimal? Balance);
