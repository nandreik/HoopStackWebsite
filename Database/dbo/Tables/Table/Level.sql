CREATE TABLE [dbo].[Level]
(
	[LevelNum] INT NOT NULL, 
    [NumStacks] INT NOT NULL, 
    [Stack1] NVARCHAR(100) NOT NULL, 
    [Stack2] NVARCHAR(100) NOT NULL, 
    [Stack3] NVARCHAR(100) NOT NULL, 
    [Stack4] NVARCHAR(100) NOT NULL, 
    [Stack5] NVARCHAR(100) NULL, 
    [Stack6] NVARCHAR(100) NULL, 
    [Stack7] NVARCHAR(100) NULL, 
    [Stack8] NVARCHAR(100) NULL, 
    [Stack9] NVARCHAR(100) NULL, 
    [Stack10] NVARCHAR(100) NULL, 
    PRIMARY KEY ([LevelNum], [NumStacks], [Stack1], [Stack2], [Stack3], [Stack4])
)