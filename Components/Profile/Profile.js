// Profile.js

import React from 'react';

const Profile = () => {
  // For now, let's assume you have a static user object
  const user = {
    Id: 1,
    Name: 'John Doe',
    Email: 'johndoe@example.com',
    Phone: '123-456-7890'
  };

  // Uncomment this part and replace with actual API call when ready
  // const [user, setUser] = useState(null);

  // useEffect(() => {
  //   const fetchUserDetails = async () => {
  //     const response = await axios.get('your-api-url');
  //     setUser(response.data);
  //   };
  //   fetchUserDetails();
  // }, []);

  return (
    <div className="profile">
      <div className="user-details">
        <h2>User Details</h2>
        <p><strong>Name:</strong> {user.Name}</p>
        <p><strong>Email:</strong> {user.Email}</p>
        <p><strong>Phone:</strong> {user.Phone}</p>
      </div>
    </div>
  );
};

export default Profile;
