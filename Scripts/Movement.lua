Vec3 = require "Vec3"

PATH = {
    Vec3:New(2,0,-12),
    Vec3:New(-2,0,-12),
    Vec3:New(-2,0,-9),
    Vec3:New(2,0,-9)
}

function NextPath (E)
    E._internal.position = E._internal.path[E._internal.path_point];
    if (E._internal.path_point >= #E._internal.path) then
        E._internal.path_point = 1;
    else
        E._internal.path_point = E._internal.path_point + 1
    end
    E._internal.direction = (E._internal.path[E._internal.path_point] - E._internal.position):Normalize()
end

function Init (E)
    E._internal = {}
    E._internal.position = nil
    E._internal.direction = nil
    E._internal.path_point = 1
    E._internal.speed = 1
    E._internal.path = {
        Vec3:New(2,0,-12),
		Vec3:New(-2,0,-12),
		Vec3:New(-2,0,-9),
		Vec3:New(2,0,-9)
    }
    NextPath(E)
end

function Step (E, dt)
    E._internal.position = E._internal.position + (E._internal.direction * (E._internal.speed * dt))
    E.position_set(E._internal.position.x,
                   E._internal.position.y,
                   E._internal.position.z)
    if (E._internal.position:Near(E._internal.path[E._internal.path_point])) then
        NextPath(E)
    end
end
