USE [TreinouDb]
GO

ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_Students_Teachers_TeacherId]
GO

ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_Teachers_TeacherId] FOREIGN KEY([TeacherId])
REFERENCES [dbo].[Teachers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_Teachers_TeacherId]
GO

ALTER TABLE [dbo].[Students] ALTER COLUMN [TeacherId] UNIQUEIDENTIFIER NULL
