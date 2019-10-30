local Vec3 = { }

function Vec3:__mul (other)
    if type(other) == "number" then
        -- a scalar
        return Vec3:New(self.x * other, self.y * other, self.z * other)
    else
        -- a cross product
        error("cross product not implemented")
    end
end

function Vec3:__add (other)
    return Vec3:New(self.x + other.x, self.y + other.y, self.z + other.z)
end

function Vec3:__sub (other)
    return Vec3:New(self.x - other.x, self.y - other.y, self.z - other.z)
end

function Vec3:__tostring ()
    return "(" .. self.x .. ", " .. self.y .. ", " .. self.z .. ")"
end

function Vec3:__concat (other)
    return tostring(self) .. tostring(other)
end

function Vec3:Near (other)
    local leeway = 0.1
    local dist = self:Distance(other)
    return (dist < leeway)
end

function Vec3:Distance (other)
    return math.sqrt(math.pow(other.x - self.x, 2) +
                          math.pow(other.y - self.y, 2) +
                          math.pow(other.z - self.z, 2))
end

function Vec3:Magnitude ()
    return math.sqrt(self.x * self.x + self.y * self.y + self.z * self.z)
end

function Vec3:Normalize ()
    local length = self:Magnitude()
    return Vec3:New(self.x / length, self.y / length, self.z / length)
end

function Vec3:New (x, y, z)
    local t = {
        x = x or 0,
        y = y or 0,
        z = z or 0,
    }
    setmetatable(t, self)
    self.__index = self
    return t
end

return Vec3
