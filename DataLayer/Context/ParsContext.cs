using ParsiBin.DataLayer;
using ParsiBin.DomainClasses;
using ParsiBin.DomainClasses.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DataLayer
{
    public class ParsContext : DbContext , IUnitOfWork
    {

        public ParsContext()
            : base("ParsContext")
        {
        }
        #region DbSetProperties

        public DbSet<Tournament> Tournament { set; get; }
        public DbSet<Group> Group { set; get; }
        public DbSet<SportType> SportType { set; get; }
        public DbSet<Participant> Participant { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<Referee>  Referee { get; set; }
        public DbSet<Stadium> Stadium { get; set; }
        public DbSet<ParticipantInMatch> ParticipantInMatch { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<MatchType> MatchType { get; set; }
        public DbSet<ScoreTitle> ScoreTitle { get; set; }
        public DbSet<MatchScore> MatchScore { get; set; }
        public DbSet<ParticipantInGroups> ParticipantInGroups { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<verifyUser> verifyUser { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserInRole> UserInRole { get; set; }
        public DbSet<UserPrediction> UserPrediction { get; set; }
        public DbSet<MatchResult> MatchResult { get; set; }
        public DbSet<UserRank> UserRank { get; set; }
        public DbSet<UserPredictLog> UserPredictLog { get; set; }

        #endregion
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {            
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Configurations.Add(new ParsiBin.DomainClasses.GroupConfig());
            //modelBuilder.Configurations.Add(new DomainClasses.ParticipantConfig());
            base.OnModelCreating(modelBuilder);
        }

        #region IUnitOfWork
        
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public override int SaveChanges()
        {
            applyCorrectYeKe();
            return base.SaveChanges();
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        private void applyCorrectYeKe()
        {
            //پیدا کردن موجودیت‌های تغییر کرده
            var changedEntities = this.ChangeTracker
                                      .Entries()
                                      .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var item in changedEntities)
            {
                if (item.Entity == null) continue;

                //یافتن خواص قابل تنظیم و رشته‌ای این موجودیت‌ها
                var propertyInfos = item.Entity.GetType().GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                    ).Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                var pr = new PropertyReflector();

                //اعمال یکپارچگی نهایی
                foreach (var propertyInfo in propertyInfos)
                {
                    var propName = propertyInfo.Name;
                    var val = pr.GetValue(item.Entity, propName);
                    if (val != null)
                    {
                        var newVal = val.ToString().Replace("ی", "ی").Replace("ک", "ک");
                        if (newVal == val.ToString()) continue;
                        pr.SetValue(item.Entity, propName, newVal);
                    }
                }
            }
        }

        #endregion
        
    }
}
