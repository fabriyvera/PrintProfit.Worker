using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintProfit.Worker;
public sealed class PrintServiceOptions
{
    /// <summary>
    /// Canal operativo del Visor de eventos (impresión).
    /// </summary>
    public string LogName { get; set; } = "Microsoft-Windows-PrintService/Operational";
}