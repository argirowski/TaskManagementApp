import React from "react";
import { Pagination } from "react-bootstrap";
import { PaginationControlsProps } from "../../types/types";

const PaginationControls: React.FC<PaginationControlsProps> = ({
  page,
  totalPages,
  onPageChange,
}) => {
  let items = [];
  for (let number = 1; number <= totalPages; number++) {
    items.push(
      <Pagination.Item
        key={number}
        active={number === page}
        onClick={() => onPageChange(number)}
      >
        {number}
      </Pagination.Item>
    );
  }
  return (
    <Pagination
      size="lg"
      className="justify-content-center mt-3 custom-pagination"
    >
      {items}
    </Pagination>
  );
};

export default PaginationControls;
