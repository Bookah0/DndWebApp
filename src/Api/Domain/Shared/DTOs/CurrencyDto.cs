using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Shared.DTOs;

public class CurrencyDto
{
    [Range(0, int.MaxValue)]
    public int Brass { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Copper { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Silver { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Gold { get; set; }

    [Range(0, int.MaxValue)]
    public int Platinum { get; set; }

    [Range(0, int.MaxValue)]
    public int Electrum { get; set; }
}