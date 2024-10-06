import React from 'react';

const Cart = ({ cart, removeFromCart }) => {
  return (
    <div>
      <h1>Your Cart</h1>
      {cart.length === 0 ? (
        <p>Your cart is empty!</p>
      ) : (
        cart.map((product) => (
          <div key={product.id}>
            <h3>{product.title}</h3>
            <button onClick={() => removeFromCart(product.id)}>Remove</button>
          </div>
        ))
      )}
    </div>
  );
};

export default Cart;
