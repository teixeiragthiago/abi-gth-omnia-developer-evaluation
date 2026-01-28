CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241014011203_InitialMigrations') THEN
    CREATE TABLE "Users" (
        "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
        "Username" character varying(50) NOT NULL,
        "Password" character varying(100) NOT NULL,
        "Phone" character varying(20) NOT NULL,
        "Email" character varying(100) NOT NULL,
        "Status" character varying(20) NOT NULL,
        "Role" character varying(20) NOT NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241014011203_InitialMigrations') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20241014011203_InitialMigrations', '8.0.10');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260128013412_IncludeSaleAndSaleProduct') THEN
    ALTER TABLE "Users" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260128013412_IncludeSaleAndSaleProduct') THEN
    ALTER TABLE "Users" ADD "UpdatedAt" timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260128013412_IncludeSaleAndSaleProduct') THEN
    CREATE TABLE "Sales" (
        "Id" uuid NOT NULL,
        "SaleNumber" integer NOT NULL,
        "CustomerId" uuid NOT NULL,
        "BranchId" uuid NOT NULL,
        "TotalAmount" numeric NOT NULL,
        "IsCancelled" boolean NOT NULL,
        "CancelledAt" timestamp with time zone,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        CONSTRAINT "PK_Sales" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260128013412_IncludeSaleAndSaleProduct') THEN
    CREATE TABLE "SaleProduct" (
        "Id" uuid NOT NULL,
        "SaleId" uuid NOT NULL,
        "ProductId" uuid NOT NULL,
        "Quantity" integer NOT NULL,
        "UnitPrice" numeric NOT NULL,
        "DiscountPercentage" numeric NOT NULL,
        "DiscountAmount" numeric NOT NULL,
        "TotalAmount" numeric NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone,
        CONSTRAINT "PK_SaleProduct" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_SaleProduct_Sales_SaleId" FOREIGN KEY ("SaleId") REFERENCES "Sales" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260128013412_IncludeSaleAndSaleProduct') THEN
    CREATE INDEX "IX_SaleProduct_SaleId" ON "SaleProduct" ("SaleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260128013412_IncludeSaleAndSaleProduct') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260128013412_IncludeSaleAndSaleProduct', '8.0.10');
    END IF;
END $EF$;
COMMIT;

