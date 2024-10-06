import React, { useEffect, useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './pages/Home';
import Products from './pages/Products';
import Cart from './pages/Cart';
import ContactUs from './pages/ContactUs';
import Signup from './pages/Signup';
import Login from './pages/Login';
import './App.css';

function App() {
  const [products, setProducts] = useState([]);
  const [cart, setCart] = useState([]);
  const [users, setUsers] = useState([]); // Store users
  const [loggedInUser, setLoggedInUser] = useState(null); // Track logged-in user

  useEffect(() => {
    fetch('https://fakestoreapi.com/products')
      .then((res) => res.json())
      .then((data) => setProducts(data));
  }, []);

  const addToCart = (product) => {
    setCart((prev) => [...prev, product]);
  };

  const removeFromCart = (productId) => {
    setCart((prev) => prev.filter((item) => item.id !== productId));
  };

  const handleSignup = ({ username, password, isAdmin }) => {
    setUsers((prev) => [...prev, { username, password, isAdmin }]);
  };

  const handleLogin = ({ username, password }) => {
    const user = users.find((u) => u.username === username && u.password === password);
    if (user) {
      setLoggedInUser(user);
      alert(`Welcome, ${username}!`);
    } else {
      alert('Invalid credentials');
    }
  };

  return (
    <Router>
      <div className="App">
        <Navbar cart={cart} />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/products" element={<Products products={products} addToCart={addToCart} />} />
          <Route path="/cart" element={<Cart cart={cart} removeFromCart={removeFromCart} />} />
          <Route path="/contact" element={<ContactUs />} />
          <Route path="/signup" element={<Signup onSignup={handleSignup} />} />
          <Route path="/login" element={<Login onLogin={handleLogin} />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
