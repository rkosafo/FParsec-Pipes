﻿namespace Examples.SQLite
open System
open System.Collections.Generic
open System.Globalization

type Literal =
    | NullLiteral
    | CurrentTimeLiteral
    | CurrentDateLiteral
    | CurrentTimestampLiteral
    | StringLiteral of string
    | BlobLiteral of byte array
    | IntegerLiteral of int64
    | FloatLiteral of float

type Name = string

type TableName =
    {
        SchemaName : string option
        TableName : string
    }

type ColumnName =
    {
        Table : TableName option
        ColumnName : string
    }

type BindParameter =
    | NamedParameter of char * string // char is the prefix: ':', '@', or '$'
    | PositionalParameter of uint32 option
    
type BinaryOperator =
    | Concatenate
    | Multiply
    | Divide
    | Modulo
    | Add
    | Subtract
    | BitShiftLeft
    | BitShiftRight
    | BitAnd
    | BitOr
    | LessThan
    | LessThanOrEqual
    | GreaterThan
    | GreaterThanOrEqual
    | Equal
    | NotEqual
    | Is
    | IsNot
    | Like
    | Glob
    | Match
    | Regexp
    | And
    | Or

type UnaryOperator =
    | Negative
    | Not
    | BitNot

type Expr =
    | LiteralExpr of Literal
    | BindParameterExpr of BindParameter
    | ColumnNameExpr of ColumnName
    | FunctionInvocationExpr of FunctionInvocationExpr
    | BinaryExpr of BinaryOperator * Expr * Expr
    | UnaryExpr of UnaryOperator * Expr
    | BetweenExpr of Expr * Expr * Expr
    | NotBetweenExpr of Expr * Expr * Expr
    | InExpr of Expr * InSet
    | NotInExpr of Expr * InSet
 
and TableInvocation =
    {
        Table : TableName
        Arguments : Expr ResizeArray option // we use an option to distinguish between schema.table and schema.table()
    }

and FunctionInvocationExpr =
    {
        FunctionName : Name
        Arguments : FunctionArguments
    }

and Distinct = | Distinct

and FunctionArguments =
    | ArgumentWildcard
    | ArgumentList of (Distinct option * Expr ResizeArray)

and InSet =
    | InExpressions of Expr ResizeArray
    | InSelect of SelectStmt
    | InTable of TableInvocation

and SelectStmt =
    | SelectStmt