using System.ComponentModel.DataAnnotations;

namespace HangfireOrchestratorComponent.Data;

public class Workflow
{
    [Key]
    public Guid Id { get; set; }

    public string Task1JobId { get; set; } = string.Empty;

    public string Task2JobId { get; set; } = string.Empty;

    public string Task3JobId { get; set; } = string.Empty;

    public string Task4JobId { get; set; } = string.Empty;

    public string Task5JobId { get; set; } = string.Empty;

}
