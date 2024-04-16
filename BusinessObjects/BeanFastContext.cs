using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects
{
    public partial class BeanFastContext : DbContext
    {
        public BeanFastContext()
        {
        }

        public BeanFastContext(DbContextOptions<BeanFastContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<ProfileBodyMassIndex> BMIs { get; set; }
        public virtual DbSet<Food> Foods { get; set; }
        public virtual DbSet<Combo> Combos { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Kitchen> Kitchens { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuDetail> MenuDetails { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<SessionDetail> SessionDetails { get; set; }
        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Gift> Gifts { get; set; }
        public virtual DbSet<ExchangeGift> ExchangeGifts { get; set; }
        public virtual DbSet<OrderActivity> OrderActivities { get; set; }
        public virtual DbSet<LoyaltyCard> LoyaltyCards { get; set; }
        public virtual DbSet<CardType> CardTypes { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationDetail> NotificationDetails { get; set; }
        public virtual DbSet<Game> Games { get; set; }

        //public string GetConnectionString()
        //{
        //    IConfiguration config = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", true, true)
        //        .Build();
        //    var strConn = config["ConnectionStrings:DB"];
        //    return strConn!;
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=20.11.68.170;Initial Catalog=beanfast;User ID=sa;Password=thanh@Strong(!)P4ssw00rd;TrustServerCertificate=True;");
                optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=beanfast;User ID=sa;Password=12345;TrustServerCertificate=True;");

                //optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        #region

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.Id)
                    .HasName("PK_User");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.FullName)
                    .HasMaxLength(200);
                entity.Property(e => e.Phone)
                    .HasMaxLength(30);
                entity.Property(e => e.Email)
                    .HasMaxLength(300);
                entity.HasOne(e => e.Role)
                    .WithMany(e => e.Users)
                    .HasForeignKey(e => e.RoleId)
                    .HasConstraintName("FK_User_Role")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedUsers).HasConstraintName("FK_User_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedUsers).HasConstraintName("FK_User_User_UpdaterId");
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Role");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.Name)
                    .HasMaxLength(40);
            });
            modelBuilder.Entity<Profile>(entity =>
            {
                entity.ToTable("Profile");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Profile");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.FullName)
                    .HasMaxLength(200);
                entity.Property(e => e.NickName)
                    .HasMaxLength(50);
                entity.Property(e => e.Class)
                    .HasMaxLength(20);
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Profiles)
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("FK_Profile_User");
                entity.HasOne(e => e.School)
                    .WithMany(e => e.Profiles)
                    .HasForeignKey(e => e.SchoolId)
                    .HasConstraintName("FK_Profile_School");
            });
            modelBuilder.Entity<ProfileBodyMassIndex>(entity =>
            {
                entity.ToTable("ProfileBodyMassIndex");
                entity.HasKey(e => e.Id)
                    .HasName("PK_ProfileBodyMassIndex");
                entity.HasOne(e => e.Profile)
                    .WithMany(e => e.BMIs)
                    .HasForeignKey(e => e.ProfileId)
                    .HasConstraintName("FK_ProfileBodyMassIndex_Profile");
            });
            modelBuilder.Entity<Food>(entity =>
            {
                entity.ToTable("Food");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Food");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.Name)
                    .HasMaxLength(200);
                entity.HasOne(e => e.Category)
                    .WithMany(e => e.Foods)
                    .HasForeignKey(e => e.CategoryId)
                    .HasConstraintName("FK_Food_Category")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedFoods).HasConstraintName("FK_Food_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedFoods).HasConstraintName("FK_Food_User_UpdaterId");
            });
            modelBuilder.Entity<Combo>(entity =>
            {
                entity.ToTable("Combo");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Combo");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.HasOne(e => e.MasterFood)
                    .WithMany(e => e.MasterCombos)
                    .HasForeignKey(e => e.MasterFoodId)
                    .HasConstraintName("FK_Combo_MasterFood")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Food)
                    .WithMany(e => e.Combos)
                    .HasForeignKey(e => e.FoodId)
                    .HasConstraintName("FK_Combo_Food")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedCombos).HasConstraintName("FK_Combo_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedCombos).HasConstraintName("FK_Combo_User_UpdaterId");
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.Name)
                    .HasMaxLength(100);
                entity.ToTable("Category");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Category");
                entity.Property(e => e.Name)
                    .HasMaxLength(200);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedCategories).HasConstraintName("FK_Category_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedCategories).HasConstraintName("FK_Category_User_UpdaterId");
            });
            modelBuilder.Entity<Kitchen>(entity =>
            {
                entity.ToTable("Kitchen");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Kitchen");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.Name)
                    .HasMaxLength(200);
                entity.Property(e => e.Address)
                    .HasMaxLength(500);
                entity.HasOne(e => e.Area)
                    .WithMany(e => e.Kitchens)
                    .HasForeignKey(e => e.AreaId)
                    .HasConstraintName("FK_Kitchen_Area")
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedKitchens).HasConstraintName("FK_Kitchen_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedKitchens).HasConstraintName("FK_Kitchen_User_UpdaterId");
            });
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Menu");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.HasOne(e => e.Kitchen)
                    .WithMany(e => e.Menus)
                    .HasForeignKey(e => e.KitchenId)
                    .HasConstraintName("FK_Menu_Kitchen");
                entity.HasOne(e => e.Creator)
                    .WithMany(e => e.CreatedMenus)
                    .HasForeignKey(e => e.CreatorId)
                    .HasConstraintName("FK_Menu_User_CreatorId")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Updater)
                    .WithMany(e => e.UpdatedMenus)
                    .HasForeignKey(e => e.UpdaterId)
                    .HasConstraintName("FK_Menu_User_UpdaterId")
                    .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<MenuDetail>(entity =>
            {
                entity.ToTable("MenuDetail");
                entity.HasKey(e => e.Id)
                    .HasName("PK_MenuDetail");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.HasOne(e => e.Food)
                    .WithMany(e => e.MenuDetails)
                    .HasForeignKey(e => e.FoodId)
                    .HasConstraintName("FK_MenuDetail_Food");
                entity.HasOne(e => e.Menu)
                    .WithMany(e => e.MenuDetails)
                    .HasForeignKey(e => e.MenuId)
                    .HasConstraintName("FK_MenuDetail_Menu");
            });
            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("Session");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Session");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.HasOne(e => e.Menu)
                    .WithMany(e => e.Sessions)
                    .HasForeignKey(e => e.MenuId)
                    .HasConstraintName("FK_Session_Menu");
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedSessions).HasConstraintName("FK_Session_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedSessions).HasConstraintName("FK_Session_User_UpdaterId");
            });
            modelBuilder.Entity<SessionDetail>(entity =>
            {
                entity.ToTable("SessionDetail");
                entity.HasKey(e => e.Id)
                    .HasName("PK_SessionDetail");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.HasOne(e => e.Location)
                    .WithMany(e => e.SessionDetails)
                    .HasForeignKey(e => e.LocationId)
                    .HasConstraintName("FK_SessionDetail_Location");
                entity.HasOne(e => e.Session)
                    .WithMany(e => e.SessionDetails)
                    .HasForeignKey(e => e.SessionId)
                    .HasConstraintName("FK_SessionDetail_Session")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Deliverer)
                    .WithMany(e => e.SessionDetails)
                    .HasForeignKey(e => e.DelivererId)
                    .HasConstraintName("FK_SessionDetail_User")
                    .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<School>(entity =>
            {
                entity.ToTable("School");
                entity.HasKey(e => e.Id)
                    .HasName("PK_School");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.Name)
                    .HasMaxLength(200);
                entity.Property(e => e.Address)
                    .HasMaxLength(500);
                entity.HasOne(e => e.Area)
                    .WithMany(e => e.PrimarySchools)
                    .HasForeignKey(e => e.AreaId)
                    .HasConstraintName("FK_School_Area")
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Kitchen)
                    .WithMany(e => e.PrimarySchools)
                    .HasForeignKey(e => e.KitchenId)
                    .HasConstraintName("FK_School_Kitchen")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedSchools).HasConstraintName("FK_School_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedSchools).HasConstraintName("FK_School_User_UpdaterId");
            });
            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Location");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();
                entity.HasOne(e => e.School)
                    .WithMany(e => e.Locations)
                    .HasForeignKey(e => e.SchoolId)
                    .HasConstraintName("FK_Location_School");
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedLocations).HasConstraintName("FK_Location_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedLocations).HasConstraintName("FK_Location_User_UpdaterId");
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Order");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.HasOne(e => e.SessionDetail)
                    .WithMany(e => e.Orders)
                    .HasForeignKey(e => e.SessionDetailId)
                    .HasConstraintName("FK_Order_SessionDetail");
                entity.HasOne(e => e.Profile)
                    .WithMany(e => e.Orders)
                    .HasForeignKey(e => e.ProfileId)
                    .HasConstraintName("FK_Order_Profile")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedOrders).HasConstraintName("FK_Order_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedOrders).HasConstraintName("FK_Order_User_UpdaterId");
            });
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");
                entity.HasKey(e => e.Id)
                    .HasName("PK_OrderDetail");
                entity.HasOne(e => e.Order)
                    .WithMany(e => e.OrderDetails)
                    .HasForeignKey(e => e.OrderId)
                    .HasConstraintName("FK_OrderDetail_Order");
                entity.HasOne(e => e.Food)
                    .WithMany(e => e.OrderDetails)
                    .HasForeignKey(e => e.FoodId)
                    .HasConstraintName("FK_OrderDetail_Food");
            });
            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("Wallet");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Wallet");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.Name)
                    .HasMaxLength(100);
                entity.Property(e => e.Type)
                    .HasMaxLength(20);
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Wallets)
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("FK_Wallet_User")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Profile)
                    .WithMany(e => e.Wallets)
                    .HasForeignKey(e => e.ProfileId)
                    .HasConstraintName("FK_Wallet_Profile");
            });
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Transaction");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.HasOne(e => e.Order)
                    .WithMany(e => e.Transactions)
                    .HasForeignKey(e => e.OrderId)
                    .HasConstraintName("FK_Transaction_Order");
                entity.HasOne(e => e.ExchangeGift)
                    .WithMany(e => e.Transactions)
                    .HasForeignKey(e => e.ExchangeGiftId)
                    .HasConstraintName("FK_Transaction_ExchangeGift");
                entity.HasOne(e => e.Wallet)
                    .WithMany(e => e.Transactions)
                    .HasForeignKey(e => e.WalletId)
                    .HasConstraintName("FK_Transaction_Wallet");
                entity.HasOne(e => e.Game)
                    .WithMany(e => e.Transactions)
                    .HasConstraintName("FK_Transaction_Game");
            });
            modelBuilder.Entity<Gift>(entity =>
            {
                entity.ToTable("Gift");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Gift");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.Name)
                    .HasMaxLength(200);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedGifts).HasConstraintName("FK_Gift_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedGifts).HasConstraintName("FK_Gift_User_UpdaterId");
            });
            modelBuilder.Entity<ExchangeGift>(entity =>
            {
                entity.ToTable("ExchangeGift");
                entity.HasKey(e => e.Id)
                    .HasName("PK_ExchangeGift");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.HasOne(e => e.Profile)
                    .WithMany(e => e.ExchangeGifts)
                    .HasForeignKey(e => e.ProfileId)
                    .HasConstraintName("FK_ExchangeGift_Profile");
                entity.HasOne(e => e.SessionDetail)
                    .WithMany(e => e.ExchangeGifts)
                    .HasForeignKey(e => e.SessionDetailId)
                    .HasConstraintName("FK_ExchangeGift_SessionDetail")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Gift)
                    .WithMany(e => e.ExchangeGifts)
                    .HasForeignKey(e => e.GiftId)
                    .HasConstraintName("FK_ExchangeGift_Gift")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedExchangeGifts).HasConstraintName("FK_ExchangeGift_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedExchangeGifts).HasConstraintName("FK_ExchangeGift_User_UpdaterId");

            });
            modelBuilder.Entity<OrderActivity>(entity =>
            {
                entity.ToTable("OrderActivity");
                entity.HasKey(e => e.Id)
                    .HasName("PK_OrderActivity");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.Name)
                    .HasMaxLength(200);
                entity.HasOne(e => e.Order)
                    .WithMany(e => e.OrderActivities)
                    .HasForeignKey(e => e.OrderId)
                    .HasConstraintName("FK_OrderActivity_Order");
                entity.HasOne(e => e.ExchangeGift)
                    .WithMany(e => e.Activities)
                    .HasForeignKey(e => e.ExchangeGiftId)
                    .HasConstraintName("FK_OrderActivity_ExchangeGift")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedOrderActivities).HasConstraintName("FK_OrderActivity_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedOrderActivities).HasConstraintName("FK_OrderActivity_User_UpdaterId");
            });
            modelBuilder.Entity<LoyaltyCard>(entity =>
            {
                entity.ToTable("LoyaltyCard");
                entity.HasKey(e => e.Id)
                    .HasName("PK_LoyaltyCard");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.Title)
                    .HasMaxLength(200);
                entity.HasOne(e => e.Profile)
                    .WithMany(e => e.LoyaltyCards)
                    .HasForeignKey(e => e.ProfileId)
                    .HasConstraintName("FK_LoyaltyCard_Profile")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.CardType)
                    .WithMany(e => e.LoyaltyCards)
                    .HasForeignKey(e => e.CardTypeId)
                    .HasConstraintName("FK_LoyaltyCard_CardType")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedLoyaltyCards).HasConstraintName("FK_LoyaltyCard_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedLoyaltyCards).HasConstraintName("FK_LoyaltyCard_User_UpdaterId");

            });
            modelBuilder.Entity<CardType>(entity =>
            {
                entity.ToTable("CardType");
                entity.HasKey(e => e.Id)
                    .HasName("PK_CardType");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.Name)
                    .HasMaxLength(200);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedCardTypes).HasConstraintName("FK_CardType_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedCardTypes).HasConstraintName("FK_CardType_User_UpdaterId");
            });
            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Area");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.City)
                    .HasMaxLength(100);
                entity.Property(e => e.District)
                    .HasMaxLength(100);
                entity.Property(e => e.Ward)
                    .HasMaxLength(100);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedAreas).HasConstraintName("FK_Area_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedAreas).HasConstraintName("FK_Area_User_UpdaterId");
            });
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Notification");

            });
            modelBuilder.Entity<NotificationDetail>(entity =>
            {
                entity.ToTable("NotificationDetail");
                entity.HasKey(e => e.Id).HasName("PK_NotificationDetail");
                entity.HasOne(e => e.User)
                    .WithMany(e => e.NotificationDetails)
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("FK_NotificationDetail_User");
                entity.HasOne(e => e.Notification)
                    .WithMany(e => e.NotificationDetails)
                    .HasForeignKey(e => e.NotificationId)
                    .HasConstraintName("FK_NotificationDetail_Notification");
            });
            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");
                entity.HasKey(e => e.Id)
                    .HasName("PK_Game");
                entity.Property(e => e.Code)
                    .HasMaxLength(100);
                entity.Property(e => e.Name)
                    .HasMaxLength(200);
                entity.Property(e => e.Description)
                    .HasMaxLength(500);
                entity.HasOne(e => e.Creator).WithMany(e => e.CreatedGames).HasConstraintName("FK_Game_User_CreatorId");
                entity.HasOne(e => e.Updater).WithMany(e => e.UpdatedGames).HasConstraintName("FK_Game_User_UpdaterId");
            });
            #endregion

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}