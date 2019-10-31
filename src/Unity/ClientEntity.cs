using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace ClientEntity {
    public class Entity
    {
        public GameObject obj;
        public DynValue _internal;
        
        public Entity (GameObject obj)
        {
            this.obj = obj;
        }
    }

    public class EntityProxy
    {
        Entity target;

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

        public void position_set (float x, float y, float z)
        {
            target.obj.transform.position = new Vector3(x, y, z);
        }
    }
}
