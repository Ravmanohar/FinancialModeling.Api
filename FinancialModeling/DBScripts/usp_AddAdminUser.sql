USE [FinancialModelingDB]
GO

--User Id = dixonadmin@dixon.com, Password : Dixon@123

DECLARE @AdminUserId NVARCHAR(128) = '60e29bf1-bdfb-4e14-a9ea-eaa2634bab33';

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
GO