using ECommons.Automation.NeoTaskManager;

namespace FirstPlugin.Scheduler.Tasks
{
    internal class TaskGoToFC
    {
        internal static void Enqueue()
        {
            TaskPluginLog.Enqueue("Heading to your Free Company");
            P.taskManager.Enqueue(() => UpdateCurrentTask("Going to Fc"));
            P.taskManager.Enqueue(GoHome, configuration: LSConfig);
            P.taskManager.EnqueueDelay(1000);
            P.taskManager.Enqueue(() => UpdateCurrentTask("idle"));
        }
        private static void GoHome() => P.taskManager.InsertMulti([new(P.lifestream.TeleportToFC), new(() => P.lifestream.IsBusy()), new(() => !P.lifestream.IsBusy(), LSConfig)]);
        private static TaskManagerConfiguration LSConfig => new(timeLimitMS: 2 * 60 * 1000);
    }
}
