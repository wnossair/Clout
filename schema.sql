BEGIN TRANSACTION;
CREATE TABLE [Person] (
    [Id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [Name] text NOT NULL UNIQUE
);
CREATE TABLE [Following] (
    [PersonId] integer PRIMARY KEY NOT NULL,
    [FollowingId] integer NOT NULL,
    CONSTRAINT [FK_Following_0_0] FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id])
);
COMMIT;
