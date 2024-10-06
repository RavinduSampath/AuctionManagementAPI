import React, { useState } from 'react';

const Signup = ({ onSignup }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [isAdmin, setIsAdmin] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    onSignup({ username, password, isAdmin });
    setUsername('');
    setPassword('');
    setIsAdmin(false);
  };

  return (
    <form onSubmit={handleSubmit}>
      <h2>Signup</h2>
      <label>
        Username:
        <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} required />
      </label>
      <label>
        Password:
        <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
      </label>
      <label>
        Admin:
        <input type="checkbox" checked={isAdmin} onChange={() => setIsAdmin(!isAdmin)} />
      </label>
      <button type="submit">Signup</button>
    </form>
  );
};

export default Signup;
