namespace AsyncHelloWorld
{
    public class BreakfastProgram
    {

        static async Task Main()
        {
            Task eggsTask =  Breakfast.MakeEggs();
            Task orangeJuiceTask = Breakfast.MakeOrangeJuice();

            Task<Task[]> combinedTasks = Task.WhenAll(eggsTask, orangeJuiceTask);
            await combinedTasks;

        }
    }
}
