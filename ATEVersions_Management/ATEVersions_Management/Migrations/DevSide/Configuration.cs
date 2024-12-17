namespace ATEVersions_Management.Migrations.DevSide
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ATEVersions_Management.Models.ATEVersionModels.ATEVersionContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\DevSide";
        }

        protected override void Seed(ATEVersions_Management.Models.ATEVersionModels.ATEVersionContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
