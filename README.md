# Patient Record Management System (PRMS)

## Table of Contents

1. [Project Overview](#project-overview)
2. [Features](#features)
3. [Technologies Used](#technologies-used)
4. [Installation](#installation)
5. [Usage](#usage)
6. [API Documentation](#api-documentation)
7. [Contributing](#contributing)
8. [License](#license)
9. [Contact](#contact)

## Project Overview

The **Patient Record Management System (PRMS)** is a comprehensive application designed to manage the healthcare records of patients efficiently and securely. The system aims to provide healthcare providers with a centralized platform for storing, updating, and retrieving patient information, enabling better patient care and improving the overall management of healthcare records.

PRMS helps healthcare professionals maintain a detailed, organized record of patients, including personal information, medical history, appointments, diagnoses, treatments, and prescriptions. By integrating various features such as user roles, secure access control, and real-time data processing, PRMS aims to streamline healthcare operations and improve the overall experience for both healthcare providers and patients.

## Features

- **Patient Information Management**: Store and manage patient details such as name, age, gender, contact information, medical history, allergies, and more.
- **Appointment Scheduling**: Schedule, view, and update patient appointments, with automated reminders.
- **Medical History**: Track and maintain a detailed history of patient diagnoses, treatments, surgeries, and other health records.
- **Prescriptions**: Manage patient prescriptions, including medications, dosages, and treatment plans.
- **Doctor and Patient Dashboards**: Separate dashboards for doctors and patients to view and manage information tailored to their roles.
- **Search and Filter**: Search and filter patient records by various criteria (name, date of birth, diagnosis, etc.).
- **User Roles**: Different user roles such as Admin, Doctor, and Patient, with specific access levels for each.
- **Data Security**: Ensure that patient records are encrypted and stored securely with access controls to protect sensitive data.
- **Reporting and Analytics**: Generate reports and analyze patient data for better decision-making.

## Technologies Used

- **Backend Framework**: C# with .NET Core
- **Database**: Microsoft SQL Server
- **Payment Integration**: Paystack API
- **APIs**: RESTful services
- **Authentication**: Secure role-based access control
- **Collaboration Features**: In-app messaging

## Installation

### Prerequisites

Make sure you have the following installed on your local machine:

- **.NET Core SDK** (for the backend)
- **Microsoft SQL Server** (for development and testing)
- **Git** (for version control)

### Steps

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/PRMS.git
    ```

2. Navigate to the project directory:

    ```bash
    cd PRMS
    ```

3. Install dependencies for the backend:

    ```bash
    cd backend
    dotnet restore
    ```

4. Configure environment variables:
   - Create a `.env` file in the backend directory and add your database URL, API keys, etc.

5. Start the backend server:

    ```bash
    dotnet run
    ```

6. Open your browser and go to `http://localhost:5000` to access the backend.

## Usage

Once the system is up and running, users can:

- **Admin Users**: Manage patient records, view all appointments, generate reports, and assign user roles.
- **Doctors**: Add and update patient diagnoses, manage prescriptions, view patient medical history, and schedule appointments.
- **Patients**: View personal information, appointments, prescriptions, and contact their doctor.

### Example Use Cases

1. **Patient Registration**: A patient can be registered with personal details and medical history. The admin can add new patients through the system.
   
2. **Appointment Scheduling**: A doctor can schedule and manage appointments with patients, with automatic notifications to both the doctor and the patient.

3. **Medical Record Update**: Doctors can update patient records with new diagnoses, prescriptions, and medical procedures.

## API Documentation

For detailed API usage and endpoints, see the [API Documentation](API_DOC.md).

## Contributing

If you'd like to contribute to the development of the Patient Record Management System, please follow these steps:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature`).
3. Make your changes and commit (`git commit -am 'Add new feature'`).
4. Push to the branch (`git push origin feature/your-feature`).
5. Create a pull request explaining your changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

For any inquiries or issues, feel free to reach out:

- **Email**: ogbeidemurphy@gmail.com
- **GitHub**: [https://github.com/murphite](https://github.com/murphite)
- **Project Repository**: [https://github.com/murphite/PRMS-BE](https://github.com/murphite/PRMS-BE)
