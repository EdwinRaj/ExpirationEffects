CREATE TABLE [NseMeta].[ExpirationDetail] (
    [ExpirationId]   INT      IDENTITY (1, 1) NOT NULL,
    [ExpirationYear] CHAR (4) NOT NULL,
    [ExpirationDate] DATETIME NOT NULL,
    [SymbolId]       INT      NOT NULL,
    CONSTRAINT [PK_Expiration] PRIMARY KEY CLUSTERED ([ExpirationId] ASC),
    CONSTRAINT [FK_Expiration_Symbol] FOREIGN KEY ([SymbolId]) REFERENCES [NseMeta].[Symbol] ([SymbolId])
);

