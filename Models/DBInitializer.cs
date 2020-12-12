namespace SchoolPlanner.Models {

    public static class DBInitializer {

        public static void initialize() {
            using (var context = new SchoolPlannerContext()) {
                context.Database.EnsureCreated();
            }
        }
    }
}
