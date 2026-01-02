// Manual mock for react-router-dom
import React from "react";

export const mockNavigate = jest.fn();
export const mockUseParams = jest.fn();

export const useNavigate = () => mockNavigate;

export const useParams = () => mockUseParams();

export const MemoryRouter = ({ children }: { children: React.ReactNode }) => {
  return <>{children}</>;
};

export const BrowserRouter = ({ children }: { children: React.ReactNode }) => {
  return <>{children}</>;
};

export const Navigate = ({ to }: { to: string }) => {
  return <div data-testid={`navigate-${to}`}>Navigate to {to}</div>;
};

export const Outlet = () => {
  return <div data-testid="outlet">Outlet</div>;
};

export const Routes = ({ children }: { children: React.ReactNode }) => {
  return <div data-testid="routes">{children}</div>;
};

export const Route = ({
  path,
  element,
}: {
  path: string;
  element: React.ReactNode;
}) => {
  return <div data-testid={`route-${path}`}>{element}</div>;
};
