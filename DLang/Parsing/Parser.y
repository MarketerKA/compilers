%namespace DLang.Parsing
%using DLang.Parsing.AST;
%using DLang.Lexing;
%visibility internal

%token EOF
%token VAR FUNC IF THEN ELSE END WHILE FOR IN LOOP RETURN PRINT IS DECLARE ASSIGN
%token PLUS MINUS MULTIPLY DIVIDE EQUAL NOT_EQUAL LESS LESS_EQUAL GREATER GREATER_EQUAL AND OR XOR NOT
%token DOT COMMA SEMICOLON LPAREN RPAREN LBRACKET RBRACKET LBRACE RBRACE ARROW
%token READ_INT READ_REAL READ_STRING
%token IDENTIFIER INTEGER_LITERAL REAL_LITERAL BOOLEAN_LITERAL STRING_LITERAL
%token INT REAL BOOL STRING EMPTY VECTOR TUPLE RANGE

%union {
    public Token Token;
    public ProgramTree ProgramTree;
    public Statement Statement;
    public Declaration Declaration;
    public StatementList StatementList;
    public Expression Expression;
    public ExpressionList ExpressionList;
    public Definition Definition;
    public DefinitionList DefinitionList;
    public Assignment Assignment;
    public Print Print;
    public If If;
    public Loop Loop;
    public AST.Range Range;
    public Return Return;
    public ExpressionOperator ExpressionOperator;
    public Relation Relation;
    public RelationOperator RelationOperator;
    public Factor Factor;
    public FactorOperator FactorOperator;
    public Term Term;
    public TermOperator TermOperator;
    public Unary Unary;
    public Reference Reference;
    public Primary Primary;
    public PrimaryOperator PrimaryOperator;
    public ReadType ReadType;
    public TypeIndicator TypeIndicator;
    public Literal Literal;
    public FunctionLiteral FunctionLiteral;
    public IdentifierList IdentifierList;
    public FunctionBody FunctionBody;
    public AST.Tuple Tuple;
    public TupleElements TupleElements;
    public TupleElement TupleElement;
    public AST.Array Array;
    public ArrayElements ArrayElements;
}

%type <ProgramTree> ProgramTree
%type <Statement> Statement
%type <Declaration> Declaration
%type <StatementList> StatementList
%type <Expression> Expression
%type <ExpressionList> ExpressionList
%type <Definition> Definition
%type <DefinitionList> DefinitionList
%type <Assignment> Assignment
%type <Print> Print
%type <If> If
%type <StatementList> IfTail
%type <Loop> Loop
%type <Range> Range
%type <Return> Return
%type <ExpressionOperator> ExpressionOperator
%type <Relation> Relation
%type <RelationOperator> RelationOperator
%type <Factor> Factor
%type <FactorOperator> FactorOperator
%type <Term> Term
%type <TermOperator> TermOperator
%type <Unary> Unary
%type <Reference> Reference
%type <Primary> Primary
%type <PrimaryOperator> PrimaryOperator
%type <ReadType> Read
%type <TypeIndicator> TypeIndicator
%type <Literal> Literal
%type <FunctionLiteral> FunctionLiteral
%type <IdentifierList> IdentifierList
%type <FunctionBody> FunctionBody
%type <Tuple> Tuple
%type <TupleElements> TupleElements
%type <TupleElement> TupleElement
%type <Array> Array
%type <ArrayElements> ArrayElements

%left OR
%left XOR
%left AND
%nonassoc LESS LESS_EQUAL GREATER GREATER_EQUAL EQUAL NOT_EQUAL
%left PLUS MINUS
%left MULTIPLY DIVIDE

%start ProgramTree

%%
ProgramTree
    : StatementList { $$ = new ProgramTree($1); }
    ;

StatementList
    : Statement { $$ = new StatementList($1); }
    | StatementList Statement { $1.Add($2); $$ = $1; }
    ;

Statement
    : Declaration { $$ = new Statement($1); }
    | Assignment { $$ = new Statement($1); }
    | Print { $$ = new Statement($1); }
    | If { $$ = new Statement($1); }
    | Loop { $$ = new Statement($1); }
    | Return { $$ = new Statement($1); }
    ;

Declaration
    : VAR DefinitionList SEMICOLON
      { $$ = new Declaration($2); }
    ;

DefinitionList
    : Definition { $$ = new DefinitionList($1); }
    | DefinitionList COMMA Definition { $1.Add($3); $$ = $1; }
    ;

Definition
    : IDENTIFIER { $$ = new Definition($1.Token.Value, null); }
    | IDENTIFIER ASSIGN Expression { $$ = new Definition($1.Token.Value, $3); }
    ;

Assignment
    : IDENTIFIER ASSIGN Expression SEMICOLON
      { $$ = new Assignment($1.Token.Value, $3); }
    ;

Print
    : PRINT ExpressionList SEMICOLON
      { $$ = new Print($2); }
    ;

If
    : IF Expression THEN StatementList IfTail
      { $$ = new If($2, $4, $5); }
    ;

IfTail
    : END { $$ = null; }
    | ELSE StatementList END { $$ = $2; }
    ;

Loop
    : WHILE Expression LOOP StatementList END { $$ = new Loop($2, $4); }
    | FOR Range LOOP StatementList END { $$ = new Loop(null, $2, $4); }
    | FOR IDENTIFIER IN Range LOOP StatementList END { $$ = new Loop($2.Token.Value, $4, $6); }
    ;

Range
    : Expression RANGE Expression { $$ = new AST.Range($1, $3); }
    ;

Return
    : RETURN SEMICOLON { $$ = new Return(null); }
    | RETURN Expression SEMICOLON { $$ = new Return($2); }
    ;

ExpressionList
    : Expression { $$ = new ExpressionList($1); }
    | ExpressionList COMMA Expression { $1.Add($3); $$ = $1; }
    ;

Expression
    : Relation { $$ = new Expression($1, null, null); }
    | Relation ExpressionOperator Relation { $$ = new Expression($1, $2, $3); }
    ;

ExpressionOperator
    : OR { $$ = ExpressionOperator.OR; }
    | AND { $$ = ExpressionOperator.AND; }
    | XOR { $$ = ExpressionOperator.XOR; }
    ;

Relation
    : Factor { $$ = new Relation($1, null, null); }
    | Factor RelationOperator Factor { $$ = new Relation($1, $2, $3); }
    ;

RelationOperator
    : LESS { $$ = RelationOperator.LESS; }
    | LESS_EQUAL { $$ = RelationOperator.LESS_EQUAL; }
    | GREATER { $$ = RelationOperator.GREATER; }
    | GREATER_EQUAL { $$ = RelationOperator.GREATER_EQUAL; }
    | EQUAL { $$ = RelationOperator.EQUAL; }
    | NOT_EQUAL { $$ = RelationOperator.NOT_EQUAL; }
    ;

Factor
    : Term { $$ = new Factor($1, null, null); }
    | Term FactorOperator Term { $$ = new Factor($1, $2, $3); }
    ;

FactorOperator
    : PLUS { $$ = FactorOperator.PLUS; }
    | MINUS { $$ = FactorOperator.MINUS; }
    ;

Term
    : Unary { $$ = new Term($1, null, null); }
    | Unary TermOperator Unary { $$ = new Term($1, $2, $3); }
    ;

TermOperator
    : MULTIPLY { $$ = TermOperator.MULTIPLY; }
    | DIVIDE { $$ = TermOperator.DIVIDE; }
    ;

Unary
    : Reference { $$ = new Unary($1); }
    | Reference IS TypeIndicator { $$ = new Unary($1, $3); }
    | Primary  { $$ = new Unary($1); }
    ;

Reference
    : IDENTIFIER { $$ = new Reference($1.Token.Value); }
    | Reference LBRACKET Expression RBRACKET { $$ = new Reference($1, $3); }
    | Reference LPAREN ExpressionList RPAREN { $$ = new Reference($1, $3); }
    | Reference DOT IDENTIFIER { $$ = new Reference($1, $3.Token.Value); }
    ;

Primary
    : Literal { $$ = new Primary($1); }
    | Read { $$ = new Primary($1); }
    | FunctionLiteral { $$ = new Primary($1); }
    | LPAREN Expression RPAREN { $$ = new Primary($2); }
    | PrimaryOperator Primary { $$ = new Primary($1, $2); }
    ;

PrimaryOperator
    : PLUS { $$ = PrimaryOperator.PLUS; }
    | MINUS { $$ = PrimaryOperator.MINUS; }
    | NOT { $$ = PrimaryOperator.NOT; }
    ;

Read
    : READ_INT { $$ = ReadType.INT; }
    | READ_REAL { $$ = ReadType.REAL; }
    | READ_STRING { $$ = ReadType.STRING; }
    ;

TypeIndicator
    : INT { $$ = TypeIndicator.INT; }
    | REAL { $$ = TypeIndicator.REAL; }
    | BOOL { $$ = TypeIndicator.BOOL; }
    | STRING { $$ = TypeIndicator.STRING; }
    | EMPTY { $$ = TypeIndicator.EMPTY; }
    | LBRACKET RBRACKET { $$ = TypeIndicator.ARRAY; }
    | LBRACE RBRACE { $$ = TypeIndicator.TUPLE; }
    | FUNC { $$ = TypeIndicator.FUNC; }
    ;

Literal
    : INTEGER_LITERAL { $$ = new Literal(Int128.Parse($1.Token.Value)); }
    | REAL_LITERAL { $$ = new Literal(double.Parse($1.Token.Value)); }
    | BOOLEAN_LITERAL { $$ = new Literal(bool.Parse($1.Token.Value)); }
    | STRING_LITERAL { $$ = new Literal($1.Token.Value); }
    | Tuple { $$ = new Literal($1); }
    | Array { $$ = new Literal($1); }
    | EMPTY { $$ = new Literal(); }
    ;

FunctionLiteral
    : FUNC FunctionBody { $$ = new FunctionLiteral(null, $2); }
    | FUNC LPAREN IdentifierList RPAREN FunctionBody { $$ = new FunctionLiteral($3, $5); }
    ;

IdentifierList
    : IDENTIFIER { $$ = new IdentifierList($1.Token.Value); }
    | IdentifierList COMMA IDENTIFIER { $1.Add($3.Token.Value); $$ = $1; }
    ;

FunctionBody
    : IS StatementList END { $$ = new FunctionBody($2); }
    | ARROW Expression { $$ = new FunctionBody($2); }
    ;

Tuple
    : LBRACE TupleElements RBRACE { $$ = new AST.Tuple($2); }
    | LBRACE RBRACE { $$ = new AST.Tuple(new TupleElements()); }
    ;

TupleElements
    : TupleElement { $$ = new TupleElements($1); }
    | TupleElements COMMA TupleElement { $1.Add($3); $$ = $1; }
    ;

TupleElement
    : Expression { $$ = new TupleElement(null, $1); }
    | IDENTIFIER ASSIGN Expression { $$ = new TupleElement($1.Token.Value, $3); }
    ;

Array
    : LBRACKET ArrayElements RBRACKET { $$ = new AST.Array($2); }
    | LBRACKET RBRACKET { $$ = new AST.Array(new ArrayElements()); }
    ;

ArrayElements
    : Expression { $$ = new ArrayElements($1); }
    | ArrayElements COMMA Expression { $1.Add($3); $$ = $1; }
    ;
%%

public Parser(Scanner scanner) : base(scanner) { }

public ProgramTree GetProgramTree() { return CurrentSemanticValue.ProgramTree; }
