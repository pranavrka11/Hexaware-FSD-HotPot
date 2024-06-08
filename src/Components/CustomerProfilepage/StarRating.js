import React, { useState } from 'react';
import { BsStar, BsStarFill } from 'react-icons/bs';

const StarRating = ({ onChange }) => {
  const [rating, setRating] = useState(0);

  const handleClick = (value) => {
    setRating(value);
    onChange(value);
  };

  return (
    <div>
      {[1, 2, 3, 4, 5].map((value) => (
        <span key={value} onClick={() => handleClick(value)}>
          {value <= rating ? <BsStarFill /> : <BsStar />}
        </span>
      ))}
    </div>
  );
};

export default StarRating;
