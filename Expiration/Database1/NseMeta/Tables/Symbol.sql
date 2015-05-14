CREATE TABLE [NseMeta].[Symbol] (
    [SymbolId]         INT           IDENTITY (1, 1) NOT NULL,
    [SymbolName]       VARCHAR (200) NOT NULL,
    [DerivativeTypeId] INT           NOT NULL,
    CONSTRAINT [PK_Symbol] PRIMARY KEY CLUSTERED ([SymbolId] ASC),
    CONSTRAINT [FK_Symbol_DerivativeType] FOREIGN KEY ([DerivativeTypeId]) REFERENCES [NseMeta].[DerivativeType] ([DerivativeTypeId])
);

