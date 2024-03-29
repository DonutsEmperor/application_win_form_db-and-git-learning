namespace application_win_form_db
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 

        private static IServiceProvider? _serviceProvider;

		[STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            var services = new ServiceCollection();
            services.Init();

			_serviceProvider = services.BuildServiceProvider();

			using (var db = new AppDbContext(false))  //check this for understanding
			{
				DatabaseInspector inspector = new(db);
				inspector.InspectDatabase();
			};

			ApplicationConfiguration.Initialize();
			Application.Run(_serviceProvider.GetService<Entrance>());
		}
    }

    internal static class DIExtensions
	{
        internal static void Init(this ServiceCollection services)
        {
            services.AddForms();
            services.AddServices();
        }
        
        private static void AddForms(this ServiceCollection services)
		{
            services.AddSingleton<Entrance>();
			services.AddTransient<Main>();
			services.AddTransient<CRUD_db>();
			services.AddTransient<Analytics>();
		}

		private static void AddServices(this ServiceCollection services)
		{
			services.AddDbContext<AppDbContext>();  //check this for understanding

            services.AddScoped<IDbWorker, RealDbWorker>();
			services.AddSingleton<IUserIdentity, UserIdentity>();
		}
	}
}