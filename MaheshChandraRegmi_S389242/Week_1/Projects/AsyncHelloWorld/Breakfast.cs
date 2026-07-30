namespace AsyncHelloWorld {

    public static class Breakfast {


        public static async Task MakeEggs() {
            Console.WriteLine("Eggs order received!");

            await Task.Delay(1000);
            Console.WriteLine("Eggs cracked!");

            await Task.Delay(2000);
            Console.WriteLine("Cooked omelette!");

            await Task.Delay(1500);
            Console.WriteLine("Sending order back!");
        }

        public static async Task MakeOrangeJuice() {
            Console.WriteLine("Orange Juice Order received!");

            await Task.Delay(3000);
            Console.WriteLine("Oranges added in juicer!");

            await Task.Delay(5000);
            Console.WriteLine("Juicing complete!");

            await Task.Delay(1500);
            Console.WriteLine("Juice complete!");
        }

    }

}
