CREATE TABLE [dbo].[CPUs] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Segment]      NVARCHAR (MAX) NULL,
    [Multiplier]   BIT            NULL,
    [Arch]         NVARCHAR (MAX) NULL,
    [TDP]          REAL           NOT NULL,
    [TempLimit]    REAL           NOT NULL,
    [APU]          BIT            NULL,
    [Name]         NVARCHAR (MAX) NULL,
    [BenchPoints]  REAL           NOT NULL,
    [Rank]         INT            NOT NULL,
    [SocketId]     INT            NOT NULL,
    [BrandId]      INT            NOT NULL,
    [ReleaseDate]  INT             NOT NULL,
    [Cores]        INT             NOT NULL,
    [Threads]      INT             NOT NULL,
    [FreqDefault]  INT             NOT NULL,
    [FreqTurbo]    INT             NOT NULL,
    [Techproccess] INT             NOT NULL,
    [L1cache]      INT             NOT NULL,
    [L2cache]      INT             NOT NULL,
    [L3cache]      INT             NOT NULL,
    CONSTRAINT [PK_dbo.CPUs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CPUs_dbo.Brands_BrandId] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brands] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.CPUs_dbo.Sockets_SocketId] FOREIGN KEY ([SocketId]) REFERENCES [dbo].[Sockets] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_SocketId]
    ON [dbo].[CPUs]([SocketId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BrandId]
    ON [dbo].[CPUs]([BrandId] ASC);

