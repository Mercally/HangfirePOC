using Hangfire;
using HangfireOrchestratorComponent.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HangfireOrchestratorComponent.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkFlowOneController : ControllerBase
{

    private readonly ILogger<WorkFlowOneController> _logger;
    private readonly PocDbContext _dbContext;

    public WorkFlowOneController(PocDbContext pocDbContext, ILogger<WorkFlowOneController> logger)
    {
        _logger = logger;
        _dbContext = pocDbContext;
    }

    [HttpPost("StartSimpleWorkflow")]
    public IActionResult StartSimpleWorkflow()
    {
        string taskJob1 = BackgroundJob.Enqueue(() => Debug.WriteLine("Task 1: Workflow started!"));

        string taskJob2 = BackgroundJob.ContinueJobWith(taskJob1, () => TaskToRun("Task 2"));

        string taskJob3 = BackgroundJob.ContinueJobWith(taskJob2, () => TaskToRun("Task 3"));

        string taskJob4 = BackgroundJob.ContinueJobWith(taskJob3, () => TaskToRun("Task 4"));

        BackgroundJob.ContinueJobWith(taskJob4, () => Debug.WriteLine("Task 5: Workflow completed!"));

        Debug.WriteLine("Result: OK");
        return Ok();
    }

    [HttpPost("StartComplexWorkflow")]
    public IActionResult StartComplexWorkflow()
    {
        Guid workflowId = Guid.NewGuid();

        string taskJob1 = BackgroundJob.Enqueue(() => Debug.WriteLine("Task 1: Workflow started!"));

        string taskJob2 = BackgroundJob.ContinueJobWith(taskJob1, () => TaskToEnqueue("Task 2", workflowId, nameof(Workflow.Task3JobId)));

        string taskJob3 = BackgroundJob.Schedule(() => TaskToEnqueue("Task 3", workflowId, nameof(Workflow.Task4JobId)), TimeSpan.FromDays(365));

        string taskJob4 = BackgroundJob.Schedule(() => TaskToRun("Task 4"), TimeSpan.FromDays(365));

        string taskJob5 = BackgroundJob.ContinueJobWith(taskJob4, () => Debug.WriteLine("Task 5: Workflow completed!"));

        _dbContext.Workflows.Add(new()
        {
            Id = workflowId,
            Task1JobId = taskJob1,
            Task2JobId = taskJob2,
            Task3JobId = taskJob3,
            Task4JobId = taskJob4,
            Task5JobId = taskJob5
        });

        _dbContext.SaveChanges();

        Debug.WriteLine("Result: OK");
        return Ok();
    }

    public Task TaskToRun(string taskName)
    {
        Debug.WriteLine("Started.", taskName);
        Debug.WriteLine("Processing.", taskName);
        Thread.Sleep(TimeSpan.FromSeconds(Random.Shared.NextInt64(5, 10)));
        Debug.WriteLine("Finished.", taskName);

        return Task.CompletedTask;
    }

    public async Task TaskToEnqueue(string taskName, Guid workflowId, string nextJob)
    {
        Debug.WriteLine("Started.", taskName);
        Debug.WriteLine("Processing.", taskName);
        Thread.Sleep(TimeSpan.FromSeconds(Random.Shared.NextInt64(5, 10)));
        Debug.WriteLine("Finished.", taskName);

        Workflow workflow = await _dbContext.Workflows.FirstAsync(workflow => workflow.Id == workflowId);

        Debug.WriteLine($"Triggering next job [{nextJob}].", taskName);

        switch (nextJob)
        {
            case nameof(Workflow.Task1JobId):
                BackgroundJob.Requeue(workflow.Task1JobId);
                break;
            case nameof(Workflow.Task2JobId):
                BackgroundJob.Requeue(workflow.Task2JobId);
                break;
            case nameof(Workflow.Task3JobId):
                BackgroundJob.Requeue(workflow.Task3JobId);
                break;
            case nameof(Workflow.Task4JobId):
                BackgroundJob.Requeue(workflow.Task4JobId);
                break;
            case nameof(Workflow.Task5JobId):
                BackgroundJob.Requeue(workflow.Task5JobId);
                break;
            default:
                throw new NotImplementedException();
        }
    }
}