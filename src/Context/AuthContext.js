import React, { createContext, useContext, useState, useEffect } from 'react';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState(() => {
    const token = sessionStorage.getItem('Token');
    return !!token;
  });

  useEffect(() => {
    const token = sessionStorage.getItem('Token');
    setIsLoggedIn(!!token);
  }, []);

  const handleLogin = () => {
    setIsLoggedIn(true);
    sessionStorage.setItem('Token', 'your_token_here');
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
    sessionStorage.removeItem('Token');
  };

  return (
    <AuthContext.Provider value={{ isLoggedIn, handleLogin, handleLogout }}>
      {children}
    </AuthContext.Provider>
  );
};