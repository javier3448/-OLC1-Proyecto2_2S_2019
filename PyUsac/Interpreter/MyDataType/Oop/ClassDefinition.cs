using _Compi1_Proyecto2.PyUsac.Ast.Node.Oop;
using _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker;
using _Compi1_Proyecto2.PyUsac.Interpreter.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Oop
{
    public class ClassDefinition : Definition
    {
        public string Name { get; private set; }
        public ClassNode ClassNode;
        public StaticEntity StaticEntity { get; private set; }
        public int MyType { get; private set; }

        private ClassDefinition(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Sirve para buscar en tabla class, crea una clase vacia con el nombre especificado
        /// </summary>
        public static ClassDefinition EmptyClass(string name)
        {
            return new ClassDefinition(name);
        }

        public ClassDefinition(int myType, string name, ClassNode classNode, StaticEntity staticEntity)
        {
            Name = name;
            MyType = myType;
            ClassNode = classNode;
            StaticEntity = staticEntity;
        }

        public ClassInstance CreateInstance()
        {
            return new ClassInstance(new InstanceVisitor(StaticEntity, ClassNode), this);
        }

        public override DefinitionType GetDefinitionType()
        {
            return DefinitionType.Class;
        }
    }

    public class ClassDefinitionSameName : EqualityComparer<ClassDefinition>
    {
        public override bool Equals(ClassDefinition c0, ClassDefinition c1)
        {
            if (c0 == null && c1 == null)
                return true;
            else if (c0 == null || c1 == null)
                return false;

            return (c0.Name.Equals(c1.Name));
        }

        //No se si esta implementacion produce muchas o pocas colisiones :/
        public override int GetHashCode(ClassDefinition obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
