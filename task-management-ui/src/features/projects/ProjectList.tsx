import React, { useState, useEffect } from "react";
import { Container, Row, Col, Table, Button, Card } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { Project } from "../../types/types";
import { fetchProjects, deleteProject } from "../../services/projectService";
import ConfirmDialog from "../../components/common/ConfirmDialogComponent";
import LoaderComponent from "../../components/common/LoaderComponent";
import AlertComponent from "../../components/common/AlertComponent";
import EmptyStateComponent from "../../components/common/EmptyStateComponent";

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
    loadProjects();
  }, []);

  const loadProjects = async () => {
    try {
      setLoading(true);
      const data = await fetchProjects();
      setProjects(data);
    } catch (error: any) {
      setAlertMessage("Failed to load projects. Please try again.");
      setAlertVariant("danger");
      setShowAlert(true);
    } finally {
      setLoading(false);
    }
  };

  const handleView = (project: Project) => {
    navigate(`/projects/${project.id}`);
  };

  const handleDeleteClick = (project: Project) => {
    setProjectToDelete(project);
    setShowDeleteModal(true);
  };

  const handleDeleteConfirm = async () => {
    if (!projectToDelete) return;

    try {
      await deleteProject(projectToDelete.id);
      setProjects(projects.filter((p) => p.id !== projectToDelete.id));
      setAlertMessage("Project deleted successfully!");
      setAlertVariant("success");
      setShowAlert(true);
    } catch (error: any) {
      setAlertMessage("Failed to delete project. Please try again.");
      setAlertVariant("danger");
      setShowAlert(true);
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
    return <LoaderComponent message="Loading projects..." />;
  }

  return (
    <Container>
      <Row>
        <Col>
          <Card>
            <Card.Header className="project-card-header d-flex justify-content-between align-items-center">
              <h4 className="project-header-title mb-0">Projects</h4>
              <Button
                onClick={() => navigate("/projects/new")}
                className="btn-add-project"
              >
                Add New Project
              </Button>
            </Card.Header>
            <Card.Body>
              <AlertComponent
                show={showAlert}
                variant={alertVariant}
                message={alertMessage}
                onClose={() => setShowAlert(false)}
              />

              {projects.length === 0 ? (
                <EmptyStateComponent
                  title="No projects found"
                  message="Create your first project to get started!"
                  actionText="Create Project"
                  onAction={() => navigate("/projects/new")}
                />
              ) : (
                <Table responsive className="projects-table">
                  <thead>
                    <tr>
                      <th className="table-header">Project Name</th>
                      <th className="table-header">Description</th>
                      <th className="table-header" style={{ width: "200px" }}>
                        Actions
                      </th>
                    </tr>
                  </thead>
                  <tbody>
                    {projects.map((project) => (
                      <tr key={project.id}>
                        <td className="project-name-text">
                          {project.projectName}
                        </td>
                        <td className="project-description-text">
                          {project.projectDescription || "No description"}
                        </td>
                        <td>
                          <div className="d-grid gap-2 d-md-flex">
                            <Button
                              size="sm"
                              className="btn-project-view"
                              onClick={() => handleView(project)}
                            >
                              View
                            </Button>
                            <Button
                              size="sm"
                              className="btn-edit-project"
                              onClick={() =>
                                navigate(`/projects/${project.id}/edit`)
                              }
                            >
                              Edit
                            </Button>
                            <Button
                              size="sm"
                              className="btn-project-delete"
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

      {/* Delete Confirmation Dialog */}
      <ConfirmDialog
        show={showDeleteModal}
        title="Confirm Delete"
        message={`Are you sure you want to delete the project "${projectToDelete?.projectName}"?`}
        confirmText="Delete"
        cancelText="Cancel"
        onConfirm={handleDeleteConfirm}
        onCancel={handleDeleteCancel}
        variant="danger"
      />
    </Container>
  );
};

export default ProjectList;
