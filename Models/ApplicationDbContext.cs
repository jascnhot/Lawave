using System;
using System.Data.Entity;
using System.Linq;

namespace Lawave.Models
{
    public class ApplicationDbContext : DbContext
    {
        // 您的內容已設定為使用應用程式組態檔 (App.config 或 Web.config)
        // 中的 'ApplicationDbContext' 連接字串。根據預設，這個連接字串的目標是
        // 您的 LocalDb 執行個體上的 'Lawave.Models.ApplicationDbContext' 資料庫。
        // 
        // 如果您的目標是其他資料庫和 (或) 提供者，請修改
        // 應用程式組態檔中的 'ApplicationDbContext' 連接字串。
        public ApplicationDbContext()
            : base("name=LawaveDbContext")
        {
        }

        // 針對您要包含在模型中的每種實體類型新增 DbSet。如需有關設定和使用
        // Code First 模型的詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=390109。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<lawyerAccount> lawyerAccounts { get; set; }
        public virtual DbSet<publicAccount> publicAccounts { get; set; }
        public virtual DbSet<lawyerGoodAtType> lawyerGoodAtTypes { get; set; }
        public virtual DbSet<goodAtInfo> goodAtInfoes { get; set; }
        public virtual DbSet<lawyerArea> lawyerAreas { get; set; }
        public virtual DbSet<lawyerBlacklist> lawyerBlacklists { get; set; }
        public virtual DbSet<publicCollection> publicCollections { get; set; }
        public virtual DbSet<appointmentlist> appointmentlists { get; set; }
        public virtual DbSet<FAQ> FAQs { get; set; }
        public virtual DbSet<LawyerExperience> LawyerExperiences { get; set; }
        public virtual DbSet<LawyerEducation> LawyerEducations { get; set; }
        public virtual DbSet<Chatlog> Chatlog { get; set; }
        public virtual DbSet<snsVerification> SnsVerification { get; set; }
        public virtual DbSet<mailVerification> MailVerification { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}