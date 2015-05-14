CREATE TABLE [NseMeta].[DerivativeType] (
    [DerivativeTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [DerviativeType]   VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_DerivativeType] PRIMARY KEY CLUSTERED ([DerivativeTypeId] ASC)
);

