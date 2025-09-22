import React, { useState, useEffect } from "react";
import {
  Container,
  Row,
  Col,
  Table,
  Button,
  Alert,
  Spinner,
  Card,
  Modal,
} from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import axios from "axios";

interface Project {
  id: number;
  projectName: string;
  projectDescription: string;
  createdAt?: string;
  updatedAt?: string;
}

const ProjectList: React.FC = () => {
  const navigate = useNavigate();
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);
  const [showAlert, setShowAlert] = useState(false);
  const [alertMessage, setAlertMessage] = useState("");
  const [alertVariant, setAlertVariant] = useState<"success" | "danger">(
    "success"
  );
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [projectToDelete, setProjectToDelete] = useState<Project | null>(null);

  useEffect(() => {
    fetchProjects();
  }, []);

  const fetchProjects = async () => {
    try {
      setLoading(true);
      const response = await axios.get("https://localhost:7272/api/Projects");
      setProjects(response.data);
    } catch (error: any) {
      console.error("Error fetching projects:", error);
      setAlertMessage("Failed to load projects. Please try again.");
      setAlertVariant("danger");
      setShowAlert(true);
      setTimeout(() => setShowAlert(false), 5000);
    } finally {
      setLoading(false);
    }
  };

  const handleView = (project: Project) => {
    // Navigate to project details page
    navigate(`/projects/${project.id}`);
  };

  const handleDeleteClick = (project: Project) => {
    setProjectToDelete(project);
    setShowDeleteModal(true);
  };

  const handleDeleteConfirm = async () => {
    if (!projectToDelete) return;

    try {
      await axios.delete(
        `https://localhost:7272/api/Projects/${projectToDelete.id}`
      );
      setProjects(projects.filter((p) => p.id !== projectToDelete.id));
      setAlertMessage("Project deleted successfully!");
      setAlertVariant("success");
      setShowAlert(true);
      setTimeout(() => setShowAlert(false), 3000);
    } catch (error: any) {
      console.error("Error deleting project:", error);
      setAlertMessage("Failed to delete project. Please try again.");
      setAlertVariant("danger");
      setShowAlert(true);
      setTimeout(() => setShowAlert(false), 5000);
    } finally {
      setShowDeleteModal(false);
      setProjectToDelete(null);
    }
  };

  const handleDeleteCancel = () => {
    setShowDeleteModal(false);
    setProjectToDelete(null);
  };

  if (loading) {
    return (
      <Container
        className="d-flex justify-content-center align-items-center"
        style={{ minHeight: "100vh" }}
      >
        <div className="text-center">
          <Spinner animation="border" variant="primary" />
          <p className="mt-3">Loading projects...</p>
        </div>
      </Container>
    );
  }

  return (
    <Container className="mt-4">
      <Row>
        <Col>
          <Card>
            <Card.Header className="bg-primary text-white d-flex justify-content-between align-items-center">
              <h4 className="mb-0">Projects</h4>
              <Button
                variant="light"
                size="sm"
                onClick={() => navigate("/projects/new")}
              >
                Add New Project
              </Button>
            </Card.Header>
            <Card.Body>
              {showAlert && (
                <Alert
                  variant={alertVariant}
                  dismissible
                  onClose={() => setShowAlert(false)}
                >
                  {alertMessage}
                </Alert>
              )}

              {projects.length === 0 ? (
                <div className="text-center py-5">
                  <h5>No projects found</h5>
                  <p className="text-muted">
                    Create your first project to get started!
                  </p>
                  <Button
                    variant="primary"
                    onClick={() => navigate("/projects/new")}
                  >
                    Create Project
                  </Button>
                </div>
              ) : (
                <Table responsive striped hover>
                  <thead>
                    <tr>
                      <th>Project Name</th>
                      <th>Description</th>
                      <th style={{ width: "200px" }}>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {projects.map((project) => (
                      <tr key={project.id}>
                        <td>
                          <strong>{project.projectName}</strong>
                        </td>
                        <td>
                          {project.projectDescription || "No description"}
                        </td>
                        <td>
                          <div className="d-grid gap-2 d-md-flex">
                            <Button
                              variant="outline-primary"
                              size="sm"
                              onClick={() => handleView(project)}
                            >
                              View
                            </Button>
                            <Button
                              variant="outline-danger"
                              size="sm"
                              onClick={() => handleDeleteClick(project)}
                            >
                              Delete
                            </Button>
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </Table>
              )}
            </Card.Body>
          </Card>
        </Col>
      </Row>

      {/* Delete Confirmation Modal */}
      <Modal show={showDeleteModal} onHide={handleDeleteCancel}>
        <Modal.Header closeButton>
          <Modal.Title>Confirm Delete</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          Are you sure you want to delete the project "
          {projectToDelete?.projectName}"?
          <br />
          <small className="text-muted">This action cannot be undone.</small>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleDeleteCancel}>
            Cancel
          </Button>
          <Button variant="danger" onClick={handleDeleteConfirm}>
            Delete
          </Button>
        </Modal.Footer>
      </Modal>
    </Container>
  );
};

export default ProjectList;
