CREATE PROCEDURE usp_LoadInitialData    
AS    
BEGIN    
    
 TRUNCATE TABLE LuModelTypes    
 INSERT INTO LuModelTypes    
 SELECT [value] AS ModelTypeName, 1 FROM OPENJSON('["Hourly","Time of the day","Escalating"]')    
    
 TRUNCATE TABLE LuParkingTypes    
 INSERT INTO LuParkingTypes    
 SELECT [value] AS ParkingTypeName, 1 FROM OPENJSON('["On Street","Off Street","Garages"]')    
     
 INSERT INTO AspNetRoles VALUES(1, 'Admin')    
 INSERT INTO AspNetRoles VALUES(2, 'User')    


 DECLARE @AdminUserId NVARCHAR(128) = '60e29bf1-bdfb-4e14-a9ea-eaa2634bab33';

--INSERT Admin User and Role
INSERT INTO [dbo].[AspNetUsers]
           ([Id]
           ,[ClientId]
           ,[Email]
           ,[EmailConfirmed]
           ,[PasswordHash]
           ,[SecurityStamp]
           ,[PhoneNumber]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[LockoutEndDateUtc]
           ,[LockoutEnabled]
           ,[AccessFailedCount]
           ,[UserName]
           ,[FirstName]
           ,[LastName]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[IsActive])
     VALUES
           (@AdminUserId
           ,0
           ,'dixonadmin@dixon.com'
           ,0
           ,'AAPyl4QmOHeb6WZEXF49fo0X7Bl0Vrf/OgL8hAmvXHtVS+EisfMHkSTvuJo0ieGCMw=='
           ,'c0f1974c-ddfd-42cb-933b-1f4f0520e717'
           ,NULL
           ,0
           ,0
           ,NULL
           ,0
           ,0
           ,'dixonadmin@dixon.com'
           ,NULL
           ,NULL
           ,NULL
           ,GETUTCDATE()
           ,NULL
           ,GETUTCDATE()
           ,1)
INSERT INTO [dbo].[AspNetUserRoles]
	(
		UserId, 
		RoleId
	)
	VALUES
	(
		@AdminUserId,
		1
	)
    
END