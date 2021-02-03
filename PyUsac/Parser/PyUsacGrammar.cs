using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Expressions;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Oop;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.Control;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.Jumpers;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.MemoryReadWrite;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.Natives;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Terminal;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Parser
{
    class PyUsacGrammar : Grammar
    {
        public PyUsacGrammar() : base(false)
        {
            #region Properties and Flags
            //Case sensitive = false. Setted by constructor
            LanguageFlags = LanguageFlags.NewLineBeforeEOF;//No deberia de ser necesario, pero no estoy 100% seguron :(
            #endregion

            #region Terminals
            //Non grammar
            var LINE_COMMENT = new CommentTerminal("LINE_COMMENT", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");
            NonGrammarTerminals.Add(LINE_COMMENT);
            var BLOCK_COMMENT = new CommentTerminal("BLOCK_COMMENT", "/*", "*/");
            NonGrammarTerminals.Add(BLOCK_COMMENT);

            //Punctuation
            KeyTerm O_PAREN = ToTerm("(", "O_PAREN"),
                C_PAREN = ToTerm(")", "C_PAREN"),
                O_BOX = ToTerm("[", "O_BOX"),
                C_BOX = ToTerm("]", "C_BOX"),
                O_CURLY = ToTerm("{", "O_CURLY"),
                C_CURLY = ToTerm("}", "C_CURLY"),
                SEMICOLON = ToTerm(";", "SEMICOLON"),
                COLON = ToTerm(":", "COLON"),
                EQ = ToTerm("=", "EQ"),
                VAR = ToTerm("var", "VAR"),
                COMMA = ToTerm(",", "COMMA"),
                DOT = ToTerm(".", "DOT"),
                FUNCTION = ToTerm("function", "FUNCTION"),
                VOID = ToTerm("void", "VOID"),
                CLASS = ToTerm("class", "CLASS"),
                NEW = ToTerm("new", "NEW");

            MarkPunctuation(O_PAREN, C_PAREN, O_BOX, C_BOX, O_CURLY, C_CURLY, SEMICOLON, COLON, EQ, VAR, COMMA, DOT, FUNCTION, VOID, CLASS, NEW);

            //Keywords
            KeyTerm LOG = ToTerm("log", "LOG"),
                ALERT = ToTerm("alert", "ALERT"),
                GRAPH = ToTerm("graph", "GRAPH"),
                IF = ToTerm("if", "IF"),
                SWITCH = ToTerm("switch", "SWITCH"),
                CASE = ToTerm("case", "CASE"),
                DEFAULT = ToTerm("default", "DEFAULT"),
                ELSE = ToTerm("else", "ELSE"),
                FOR = ToTerm("for", "FOR"),
                DO = ToTerm("do", "DO"),
                WHILE = ToTerm("while", "WHILE"),
                RETURN = ToTerm("return", "RETURN"),
                BREAK = ToTerm("break", "BREAK"),
                CONTINUE = ToTerm("continue", "CONTINUE"),
                IMPORTAR = ToTerm("importar", "IMPORTAR");//IMPORT NO ES KEYWORD PORQUE NINGUNA FUNCION NATIVA LO ES

            //Operators
            //Binary
            KeyTerm PLUS = ToTerm("+"),
                MINUS = ToTerm("-"),//Could be unary
                MULT = ToTerm("*"),
                DIV = ToTerm("/"),
                POW = ToTerm("pow"),
                GREATER = ToTerm(">"),
                LESS = ToTerm("<"),
                EQ_EQ = ToTerm("=="),
                NOT_EQ = ToTerm("<>"),
                GREATER_EQ = ToTerm(">="),
                LESS_EQ = ToTerm("<="),
                AND = ToTerm("&&"),
                OR = ToTerm("||"),
                XOR = ToTerm("^");
            //Unary
            KeyTerm NOT = ToTerm("!");
            KeyTerm PLUS_PLUS = ToTerm("++");
            KeyTerm MINUS_MINUS = ToTerm("--");

            //Constants

            //Literals
            var NUMBER_LITERAL = new NumberLiteral("NUMBER_LITERAL", NumberOptions.None, typeof(NumberLiteralNode));
            var STRING_LITERAL = new StringLiteral("STRING_LITERAL", "\"", StringOptions.AllowsAllEscapes, typeof(StringLiteralNode));
            var CHAR_LITERAL = new StringLiteral("CHAR_LITERAL", "'", StringOptions.IsChar, typeof(CharLiteralNode));
            //CHAPUZ SE CAMBIARON VARIAS CONFIGURACIONES PARA QUE LOS KEYTERM TRUE, FALSE Y NULL TENGA ASTNODE
            KeyTerm TRUE = ToTerm("true", "TRUE"),
                FALSE = ToTerm("false", "FALSE");
            TRUE.AstConfig.NodeType = typeof(BooleanLiteralNode);
            TRUE.Flags = TermFlags.None;
            FALSE.AstConfig.NodeType = typeof(BooleanLiteralNode);
            FALSE.Flags = TermFlags.None;

            KeyTerm NULL = ToTerm("null", "NULL");
            NULL.AstConfig.NodeType = typeof(NullLiteralNode);
            NULL.Flags = TermFlags.None;

            //Identifiers
            IdentifierTerminal IDENTIFIER = new IdentifierTerminal("IDENTIFIER");
            IDENTIFIER.AstConfig.NodeType = typeof(IdentifierNode);
            RegexBasedTerminal MAIN = new RegexBasedTerminal("MAIN", "main");
            MAIN.AstConfig.NodeType = typeof(IdentifierNode);
            #endregion

            #region Non terminals
            var program = new NonTerminal("program", typeof(ProgramNode));
            var stmtList = new NonTerminal("stmtList", typeof(TransientNode));//Ast Transient
                                                                              //definitions
                                                                              //Chapuz medio alto: se le agrego un # a los bnf defintion para identificarlos al hacer el arbol Ast de program
            var import = new NonTerminal("$import", typeof(ImportNode));//Chapuz medio alto: se le agrego un $ a los bnf import para identificarlos al hacer el arbol Ast de program
            var definition = new NonTerminal("#definition");//Transient
            var method = new NonTerminal("#method", typeof(MethodNode));
            var function = new NonTerminal("#function", typeof(FunctionNode));
            var class_definition = new NonTerminal("#class_definition", typeof(ClassNode));
            var class_bodyList = new NonTerminal("#class_bodyList", typeof(TransientNode));//Ast transient
            var class_body = new NonTerminal("#class_body");//transient
            var definition_stmt_or_importList = new NonTerminal("definition_stmt_or_importList", typeof(TransientNode));//Transient
            var definition_stmt_or_import = new NonTerminal("definition_stmt_or_import");//Transient
            var paramList = new NonTerminal("paramList", typeof(TransientNode));
            var param = new NonTerminal("param", typeof(Declaration));
            var argumentList = new NonTerminal("paramList", typeof(TransientNode));//Lista de expr separadas por coma
            //stmts
            var stmt = new NonTerminal("stmt");//Transient
            var decl = new NonTerminal("decl", typeof(Declaration));
            var identierList = new NonTerminal("identfierList", typeof(TransientNode));
            var assign = new NonTerminal("assign", typeof(Assignment));
            //nativas
            var log = new NonTerminal("log", typeof(LogNode));
            var alert = new NonTerminal("alert", typeof(Alert));
            var graph = new NonTerminal("graph", typeof(Graph));
            //control
            var block = new NonTerminal("block", typeof(Block));
            var if_stmt = new NonTerminal("if_stmt", typeof(IfNode));
            var if_else_stmt = new NonTerminal("if_else_stmt", typeof(IfNode));
            var else_content = new NonTerminal("else_content");//transient
            var switch_stmt = new NonTerminal("switch_stmt", typeof(SwitchNode));
            var switch_element = new NonTerminal("switch_element");//Transient
            var switch_elementList = new NonTerminal("switch_elementList", typeof(TransientNode));//AstTransient
            var switch_label = new NonTerminal("switch_label", typeof(SwitchLabelNode));
            var for_stmt = new NonTerminal("for_stmt", typeof(ForNode));
            var while_stmt = new NonTerminal("while_stmt", typeof(WhileNode));
            var do_while_stmt = new NonTerminal("do_while_stmt", typeof(DoWhileNode));
            //jump
            var jumper_stmt = new NonTerminal("jumper_stmt");//transient
            var break_stmt = new NonTerminal("break_stmt", typeof(BreakNode));
            var continue_stmt = new NonTerminal("break_stmt", typeof(ContinueNode));
            var return_stmt = new NonTerminal("break_stmt", typeof(ReturnNode));
            //Member access
            var member_access = new NonTerminal("member_access", typeof(MemberAccess));
            var member_access_segment = new NonTerminal("member_access_segment");//Transient
            var member_access_optional_segmentList = new NonTerminal("member_access_optional_segmentList", typeof(TransientNode));//Ast Transient
            var member_access_optional_segment = new NonTerminal("member_access_optional_segment");//Transient
            var identifier_access = new NonTerminal("identifier_access", typeof(IdentifierAccess));
            var index_access = new NonTerminal("index_access", typeof(IndexAccess));
            var expr_access = new NonTerminal("expr_access", typeof(ExprAccess));
            var index_accessList = new NonTerminal("index_accessList", typeof(TransientNode));//Ast Transient
            var function_access = new NonTerminal("function_access", typeof(ProcedureAccess));
            var object_creation_access = new NonTerminal("object_creation_access", typeof(ObjectCreationAccess));
            //Expressions
            var expr = new NonTerminal("expr");//Transient
            var parentesis_expr = new NonTerminal("parentesis_expr");//Transient
            var atomic_expr = new NonTerminal("atomic_expr", typeof(AtomicExpr));
            var binary_expr = new NonTerminal("binaray_expr", typeof(BinaryExpr));
            var unary_expr = new NonTerminal("unary_expr", typeof(UnaryExpr));
            var array_expr = new NonTerminal("array_expr", typeof(ArrayExpr));
            var exprList = new NonTerminal("exprList", typeof(TransientNode));//Ast Transient
            var inc_dec_expr = new NonTerminal("inc_dec_expr", typeof(IncDecExpr));
            var unary_op = new NonTerminal("unary_op");//Transient, the precendece doesnt work other wise
            var binary_op = new NonTerminal("binary_op");//Transient, the precendece doesnt work other wise
            var inc_dec_op = new NonTerminal("inc_dec_op");//Transient, the precendece doesnt work other wise
            #endregion

            #region BNF Rules
            Root = program;

            program.Rule = definition_stmt_or_importList;
            //Definitions
            definition_stmt_or_importList.Rule = MakeStarRule(definition_stmt_or_importList, definition_stmt_or_import);
            definition_stmt_or_import.Rule = stmt
                | definition
                | import;
            definition.Rule = method
                | function
                | class_definition;
            import.Rule = IMPORTAR + O_PAREN + STRING_LITERAL + C_PAREN + SEMICOLON;
            method.Rule = FUNCTION + VOID + IDENTIFIER + O_PAREN + paramList + C_PAREN + O_CURLY + stmtList + C_CURLY
                |MAIN + O_PAREN + paramList + C_PAREN + O_CURLY + stmtList + C_CURLY;//Chapuz alto para que acepte la sintaxis del metodo main
            function.Rule = FUNCTION + IDENTIFIER + O_PAREN + paramList + C_PAREN + O_CURLY + stmtList + C_CURLY;
            class_definition.Rule = CLASS + IDENTIFIER + O_CURLY + class_bodyList + C_CURLY;
            class_bodyList.Rule = MakeStarRule(class_bodyList, class_body);
            class_body.Rule = method
                | function
                | stmt;
            paramList.Rule = MakeStarRule(paramList, COMMA, param);
            param.Rule = VAR + IDENTIFIER;
            argumentList.Rule = MakeStarRule(argumentList, COMMA, expr);
            //stmts
            stmtList.Rule = MakeStarRule(stmtList, stmt);
            stmt.Rule = decl + SEMICOLON
                    | assign + SEMICOLON
                    | member_access + SEMICOLON
                    | inc_dec_expr + SEMICOLON
                    | log + SEMICOLON
                    | alert + SEMICOLON
                    | graph + SEMICOLON
                    | if_stmt
                    | if_else_stmt
                    | switch_stmt
                    | while_stmt
                    | do_while_stmt
                    | for_stmt
                    | jumper_stmt;
            //control stmts:
            block.Rule = O_CURLY + stmtList + C_CURLY;
            if_stmt.Rule = IF + O_PAREN + expr + C_PAREN + block;
            if_else_stmt.Rule = IF + O_PAREN + expr + C_PAREN + block + ELSE + else_content;
            else_content.Rule = block
                | if_stmt
                | if_else_stmt;
            switch_stmt.Rule = SWITCH + O_PAREN + expr + C_PAREN + O_CURLY + switch_elementList + C_CURLY;
            switch_elementList.Rule = MakeStarRule(switch_elementList, switch_element);
            switch_element.Rule = switch_label |
                stmt;
            switch_label.Rule = CASE + expr + COLON |
                DEFAULT + COLON;
            while_stmt.Rule = WHILE + O_PAREN + expr + C_PAREN + block;
            do_while_stmt.Rule = DO + block + WHILE + O_PAREN + expr + C_PAREN + SEMICOLON;
            for_stmt.Rule = FOR + O_PAREN + decl + SEMICOLON + expr + SEMICOLON + inc_dec_expr + C_PAREN + block
                | FOR + O_PAREN + assign + SEMICOLON + expr + SEMICOLON + inc_dec_expr + C_PAREN + block;//Chapuz medio para que puedan venir assinganciones tambien
            //jumpers:
            jumper_stmt.Rule = return_stmt
                | break_stmt
                | continue_stmt;
            return_stmt.Rule = RETURN + SEMICOLON
                | RETURN + expr + SEMICOLON;
            break_stmt.Rule = BREAK + SEMICOLON;
            continue_stmt.Rule = CONTINUE + SEMICOLON;
            //mem
            decl.Rule = VAR + identierList + index_accessList + EQ + expr
                | VAR + identierList + index_accessList;
            identierList.Rule = MakePlusRule(identierList, COMMA, IDENTIFIER);
            index_accessList.Rule = MakeStarRule(index_accessList, index_access);
            assign.Rule = member_access + EQ + expr;
            log.Rule = LOG + O_PAREN + expr + C_PAREN;
            alert.Rule = ALERT + O_PAREN + expr + C_PAREN;
            graph.Rule = GRAPH + O_PAREN + expr + COMMA + expr + C_PAREN;
            expr.Rule = atomic_expr
                | binary_expr
                | unary_expr;
            parentesis_expr.Rule = O_PAREN + expr + C_PAREN;
            atomic_expr.Rule = NUMBER_LITERAL
                | STRING_LITERAL
                | CHAR_LITERAL
                | TRUE
                | FALSE
                | NULL
                | array_expr
                | member_access
                | inc_dec_expr;
            binary_expr.Rule = expr + binary_op + expr;
            unary_expr.Rule = unary_op + expr + ReduceHere();
            array_expr.Rule = O_CURLY + exprList + C_CURLY;
            exprList.Rule = MakeStarRule(exprList, COMMA, expr);
            inc_dec_expr.Rule = member_access + PreferShiftHere() + inc_dec_op;
            member_access.Rule = member_access_segment + member_access_optional_segmentList;
            member_access_segment.Rule = identifier_access
                | function_access
                | object_creation_access
                | expr_access;
            member_access_optional_segmentList.Rule = MakeStarRule(member_access_optional_segmentList, member_access_optional_segment);
            member_access_optional_segment.Rule = DOT + identifier_access
                | DOT + function_access
                | index_access;
            identifier_access.Rule = IDENTIFIER;
            object_creation_access.Rule = NEW + IDENTIFIER + O_PAREN + /*argumentList +*/ C_PAREN;
            function_access.Rule = IDENTIFIER + O_PAREN + argumentList + C_PAREN;
            index_access.Rule = O_BOX + expr + C_BOX;
            expr_access.Rule = O_PAREN + expr + C_PAREN;
            binary_op.Rule = PLUS | MINUS | MULT | DIV | POW | GREATER | LESS | EQ_EQ | NOT_EQ | GREATER_EQ | LESS_EQ | AND | OR | XOR;
            unary_op.Rule = NOT | MINUS;
            inc_dec_op.Rule = PLUS_PLUS | MINUS_MINUS;
            #endregion

            #region Precedence
            RegisterOperators(5, OR);
            RegisterOperators(10, AND);
            RegisterOperators(20, Associativity.Neutral, EQ_EQ, LESS, LESS_EQ, GREATER, GREATER_EQ, NOT_EQ);
            RegisterOperators(30, PLUS, MINUS);
            RegisterOperators(40, MULT, DIV);
            RegisterOperators(50, Associativity.Right, POW);
            RegisterOperators(60, NOT);
            #endregion

            #region Transients
            MarkTransient(stmt, expr, parentesis_expr, binary_op, unary_op, inc_dec_op, member_access_segment, member_access_optional_segment,
                jumper_stmt, else_content, switch_element, definition, param, definition_stmt_or_import, class_body);
            #endregion
        }
    }
}
