using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace ServerEntity {
    public class Entity
    {
        public string name;
        public DynValue _internal;

        public Entity (string name)
        {
            this.name = name;
        }
    }

    public class EntityProxy
    {
        Entity target;

        [MoonSharpHidden]
        public EntityProxy ()
        {
            this.target = null;
        }

        [MoonSharpHidden]
        public EntityProxy (Entity e)
        {
            this.target = e;
        }

        public DynValue _internal
        {
            get { return target._internal; }
            set { target._internal = value; }
        }

        public string name
        {
            get { return target.name; }
            set { target.name = value; }
        }

        public void position_set (float x, float y, float z)
        {
        }
    }
}
