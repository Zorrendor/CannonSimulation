# Cannon Mechanics Test Task

## Task Description
The goal of this test task is to implement the following mechanics:

### Core Mechanics
1. **Cannon Control**:  
   - Implement functionality to control the cannon's aim and firing.
   
2. **Projectile Trajectory**:  
   - Create a trajectory system for the projectile flight.
   - Allow for changing the firing power and projectile speed.

3. **Mesh Generation**:  
   - Dynamically generate a custom mesh for the projectiles.

4. **Projectile Variations**:  
   - Projectiles should have different shapes.  
   - Use randomness to modify vertex positions for variation.

5. **Custom Physics for Flight**:  
   - Implement projectile flight using a custom physics system, without relying on `Rigidbody`.

6. **Ricochet Mechanics**:  
   - Projectiles should bounce (ricochet) off surfaces.

---

### Feedback Requirements
1. **Impact Effects**:  
   - Projectiles should leave a mark when hitting a wall (use `RenderTexture`).
   - Projectiles should explode on the final ricochet.

2. **Camera Shake**:  
   - Implement camera shake upon firing the cannon.

3. **Cannon Recoil Animation**:  
   - Add a visual recoil effect for the cannon barrel.

---

### Restrictions
- The following are **not allowed**:
  - Using `Rigidbody` for projectile simulation.
  - Using `Physics.Simulate`.
  - Using `Animator`.

---

Feel free to reach out if additional clarification is required.
