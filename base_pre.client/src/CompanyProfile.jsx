import { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { useNavigate } from 'react-router-dom';

function CompanyProfile({ customerId }) {
    const [profileData, setProfileData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [fadeIn, setFadeIn] = useState(false);
    const [currentPage, setCurrentPage] = useState(0);
    const navigate = useNavigate();

    // Root level styles for perfect centering
    const rootStyles = {
        position: 'fixed',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        width: '200%',
        height: '200%',
        maxWidth: '800px',
        maxHeight: '600px',
        zIndex: '9999',
        pointerEvents: 'all',
        margin: '0 auto',
        padding: '0 20px'
    };

    // Main container styles
    const containerStyles = {
        position: 'relative',
        width: '100%',
        height: '100%',
        background: 'rgba(0, 0, 0, 0.85)',
        backdropFilter: 'blur(10px)',
        zIndex: 10000,
        display: 'flex',
        flexDirection: 'column',
        overflow: 'hidden',
        color: '#57b3c0',
        borderRadius: '8px',
        boxShadow: '0 0 30px rgba(87, 179, 192, 0.2)'
    };

    // Header styles
    const headerStyles = {
        position: 'absolute',
        top: 0,
        left: 0,
        right: 0,
        height: '80px',
        background: 'rgba(0, 0, 0, 0.9)',
        padding: '20px 0',
        zIndex: 10001,
        borderBottom: '1px solid rgba(87, 179, 192, 0.3)',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center'
    };

    // Content area styles
    const contentStyles = {
        position: 'absolute',
        top: '80px',
        left: 0,
        right: 0,
        bottom: '80px',
        overflowY: 'auto',
        overflowX: 'hidden',
        padding: '20px',
        zIndex: 10000
    };

    // Table styles
    const tableStyles = {
        width: '100%',
        borderCollapse: 'separate',
        borderSpacing: '0 8px'
    };

    // Cell styles
    const cellStyles = {
        label: {
            width: '180px',
            padding: '16px 20px',
            color: '#57b3c0',
            fontSize: '16px',
            fontWeight: 'bold',
            textAlign: 'left',
            background: 'rgba(87, 179, 192, 0.1)',
            borderRadius: '4px 0 0 4px'
        },
        value: {
            padding: '16px 20px',
            color: '#d87930',
            fontSize: '16px',
            background: 'rgba(216, 121, 48, 0.1)',
            borderRadius: '0 4px 4px 0'
        }
    };

    // Navigation styles with improved spacing
    const navigationStyles = {
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0,
        height: '80px',
        padding: '0 40px',
        background: 'rgba(0, 0, 0, 0.95)',
        backdropFilter: 'blur(10px)',
        borderTop: '1px solid rgba(87, 179, 192, 0.3)',
        display: 'flex',
        justifyContent: 'space-around',
        alignItems: 'center',
        zIndex: 10002
    };

    // Button styles with improved accessibility
    const buttonStyles = {
        padding: '12px 24px',
        minWidth: '120px',
        background: 'rgba(87, 179, 192, 0.1)',
        border: '1px solid #57b3c0',
        color: '#57b3c0',
        fontSize: '16px',
        borderRadius: '4px',
        cursor: 'pointer',
        transition: 'all 0.3s ease',
        boxShadow: '0 0 15px rgba(87, 179, 192, 0.2)',
        zIndex: 10003,
        position: 'relative'
    };

    // Add button styles
    const addButtonStyles = {
        width: '100%',
        padding: '15px',
        background: 'rgba(87, 179, 192, 0.1)',
        border: '1px solid #57b3c0',
        color: '#57b3c0',
        fontSize: '16px',
        borderRadius: '4px',
        cursor: 'pointer',
        marginTop: '20px',
        transition: 'all 0.3s ease',
        animation: 'glow 1.5s infinite alternate',
        boxShadow: '0 0 10px rgba(87, 179, 192, 0.5)'
    };

    // Add keyframes for the glow animation
    const glowKeyframes = `
        @keyframes glow {
            from {
                box-shadow: 0 0 5px rgba(87, 179, 192, 0.5),
                           0 0 10px rgba(87, 179, 192, 0.5),
                           0 0 15px rgba(87, 179, 192, 0.5);
            }
            to {
                box-shadow: 0 0 10px rgba(87, 179, 192, 0.7),
                           0 0 20px rgba(87, 179, 192, 0.7),
                           0 0 30px rgba(87, 179, 192, 0.7);
            }
        }
    `;

    // Page counter styles
    const pageCounterStyles = {
        fontSize: '16px',
        color: '#57b3c0',
        padding: '0 40px',
        minWidth: '120px',
        textAlign: 'center'
    };

    useEffect(() => {
        async function fetchProfileData() {
            if (!customerId) return;
            try {
                setLoading(true);
                setFadeIn(false);
                const response = await fetch(`http://localhost:5000/api/ModelDbInits/GetCustomerByID/${customerId}`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' }
                });

                if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

                const data = await response.json();
                setProfileData(data);
                setTimeout(() => setFadeIn(true), 100);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        }

        fetchProfileData();
    }, [customerId]);

    // Handler for navigating to AddCompany page
    const handleAddCompany = () => {
        navigate('/add-company');
    };

    if (loading) {
        return (
            <div style={rootStyles}>
                <div style={{ ...containerStyles, justifyContent: 'center', alignItems: 'center' }}>
                    <div style={{
                        width: '50px',
                        height: '50px',
                        borderRadius: '50%',
                        border: '4px solid #57b3c0',
                        borderTopColor: 'transparent',
                        animation: 'spin 1s linear infinite'
                    }} />
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div style={rootStyles}>
                <div style={{ ...containerStyles, justifyContent: 'center', alignItems: 'center' }}>
                    <div style={{ color: '#ff4444', fontSize: '18px' }}>
                        System Error: {error}
                    </div>
                </div>
            </div>
        );
    }

    if (!profileData) {
        return (
            <div style={rootStyles}>
                <div style={{ ...containerStyles, justifyContent: 'center', alignItems: 'center' }}>
                    <div style={{ fontSize: '18px' }}>
                        No profile data detected.
                    </div>
                </div>
            </div>
        );
    }

    const { clientInformation, modelDbInit } = profileData;

    const pages = [
        {
            title: "Company Information",
            content: (
                <table style={tableStyles}>
                    <tbody>
                        <tr>
                            <td style={cellStyles.label}>Name:</td>
                            <td style={cellStyles.value}>
                                {clientInformation?.clientFirstName} {clientInformation?.clientLastName}
                            </td>
                        </tr>
                        <tr>
                            <td style={cellStyles.label}>Company:</td>
                            <td style={cellStyles.value}>
                                {clientInformation?.companyName}
                            </td>
                        </tr>
                        <tr>
                            <td style={cellStyles.label}>Phone:</td>
                            <td style={cellStyles.value}>
                                {clientInformation?.cleintPhone}
                            </td>
                        </tr>
                        <tr>
                            <td style={cellStyles.label}>Address:</td>
                            <td style={cellStyles.value}>
                                {clientInformation?.clientAddress}
                            </td>
                        </tr>
                    </tbody>
                </table>
            )
        },
        {
            title: "Model Information",
            content: modelDbInit && (
                <table style={tableStyles}>
                    <tbody>
                        <tr>
                            <td style={cellStyles.label}>Model ID:</td>
                            <td style={cellStyles.value}>
                                {modelDbInit.modelId}
                            </td>
                        </tr>
                        <tr>
                            <td style={cellStyles.label}>Timestamp:</td>
                            <td style={cellStyles.value}>
                                {new Date(modelDbInit.modelDbInitTimeStamp).toLocaleString()}
                            </td>
                        </tr>
                        <tr>
                            <td style={cellStyles.label}>Data Size:</td>
                            <td style={cellStyles.value}>
                                {modelDbInit.dataSize} bytes
                            </td>
                        </tr>
                        <tr>
                            <td style={cellStyles.label}>Product Vector:</td>
                            <td style={cellStyles.value}>
                                {modelDbInit.modelDbInitProductVector}
                            </td>
                        </tr>
                        <tr>
                            <td style={cellStyles.label}>Service Vector:</td>
                            <td style={cellStyles.value}>
                                {modelDbInit.modelDbInitServiceVector || 'N/A'}
                            </td>
                        </tr>
                    </tbody>
                </table>
            )
        },
        {
            title: "Operations Stage 1",
            content: profileData?.operationsStage1 && (
                <table style={tableStyles}>
                    <tbody>
                        <tr>
                            <td style={cellStyles.label}>Operations ID:</td>
                            <td style={cellStyles.value}>
                                {profileData.operationsStage1.operationsId}
                            </td>
                        </tr>
                        <tr>
                            <td style={cellStyles.label}>Order ID:</td>
                            <td style={cellStyles.value}>
                                {profileData.operationsStage1.operationsOrderId}
                            </td>
                        </tr>
                        <tr>
                            <td style={cellStyles.label}>Customer ID:</td>
                            <td style={cellStyles.value}>
                                {profileData.operationsStage1.operationsCustomerId}
                            </td>
                        </tr>
                        <tr>
                            <td style={cellStyles.label}>Product Vector:</td>
                            <td style={cellStyles.value}>
                                {profileData.operationsStage1.operationsStageOneProductVector}
                            </td>
                        </tr>
                        <tr>
                            <td style={cellStyles.label}>Service Vector:</td>
                            <td style={cellStyles.value}>
                                {profileData.operationsStage1.operationsStageOneServiceVector || 'N/A'}
                            </td>
                        </tr>
                    </tbody>
                </table>
            )
        },
        {
            title: "Products & Services",
            content: profileData?.operationsStage1 && (
                <div>
                    <h2 style={{ color: '#57b3c0', marginBottom: '20px' }}>Products</h2>
                    <table style={tableStyles}>
                        <tbody>
                            <tr>
                                <td style={cellStyles.label}>Product A:</td>
                                <td style={cellStyles.value}>
                                    Name: {profileData.operationsStage1.productA.productName}<br />
                                    Type: {profileData.operationsStage1.productA.productType}<br />
                                    Value: {profileData.operationsStage1.subProductA}
                                </td>
                            </tr>
                            <tr>
                                <td style={cellStyles.label}>Product B:</td>
                                <td style={cellStyles.value}>
                                    Name: {profileData.operationsStage1.productB.productName}<br />
                                    Type: {profileData.operationsStage1.productB.productType}<br />
                                    Value: {profileData.operationsStage1.subProductB}
                                </td>
                            </tr>
                            <tr>
                                <td style={cellStyles.label}>Product C:</td>
                                <td style={cellStyles.value}>
                                    Name: {profileData.operationsStage1.productC.productName}<br />
                                    Type: {profileData.operationsStage1.productC.productType}<br />
                                    Value: {profileData.operationsStage1.subProductC}
                                </td>
                            </tr>
                        </tbody>
                    </table>

                    <h2 style={{ color: '#57b3c0', margin: '30px 0 20px' }}>Services</h2>
                    <table style={tableStyles}>
                        <tbody>
                            <tr>
                                <td style={cellStyles.label}>Service A:</td>
                                <td style={cellStyles.value}>
                                    Name: {profileData.operationsStage1.serviceA.serviceName}<br />
                                    Type: {profileData.operationsStage1.serviceA.serviceType}<br />
                                    Value: {profileData.operationsStage1.subServiceA}
                                </td>
                            </tr>
                            <tr>
                                <td style={cellStyles.label}>Service B:</td>
                                <td style={cellStyles.value}>
                                    Name: {profileData.operationsStage1.serviceB.serviceName}<br />
                                    Type: {profileData.operationsStage1.serviceB.serviceType}<br />
                                    Value: {profileData.operationsStage1.subServiceB}
                                </td>
                            </tr>
                            <tr>
                                <td style={cellStyles.label}>Service C:</td>
                                <td style={cellStyles.value}>
                                    Name: {profileData.operationsStage1.serviceC.serviceName}<br />
                                    Type: {profileData.operationsStage1.serviceC.serviceType}<br />
                                    Value: {profileData.operationsStage1.subServiceC}
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            )
        }
    ];

    return (
        <>
            <style>{glowKeyframes}</style>
            <div style={rootStyles}>
                <div style={containerStyles}>
                    <div style={headerStyles}>
                        <h1 style={{
                            fontSize: '32px',
                            fontWeight: 'bold',
                            textAlign: 'center',
                            color: '#57b3c0',
                            margin: 0
                        }}>
                            {pages[currentPage].title}
                        </h1>
                    </div>

                    <div style={{
                        ...contentStyles,
                        opacity: fadeIn ? 1 : 0,
                        transition: 'opacity 500ms'
                    }}>
                        {pages[currentPage].content}
                    </div>

                    <div style={navigationStyles}>
                        <button
                            onClick={() => setCurrentPage(p => p - 1)}
                            disabled={currentPage === 0}
                            style={{
                                ...buttonStyles,
                                opacity: currentPage === 0 ? 0.5 : 1,
                                cursor: currentPage === 0 ? 'not-allowed' : 'pointer'
                            }}
                        >
                            Previous
                        </button>

                        <div style={pageCounterStyles}>
                            Page {currentPage + 1} of {pages.length}
                        </div>

                        <button
                            onClick={() => setCurrentPage(p => p + 1)}
                            disabled={currentPage === pages.length - 1}
                            style={{
                                ...buttonStyles,
                                opacity: currentPage === pages.length - 1 ? 0.5 : 1,
                                cursor: currentPage === pages.length - 1 ? 'not-allowed' : 'pointer'
                            }}
                        >
                            Next
                        </button>
                    </div>
                </div>
                <button
                    style={addButtonStyles}
                    onClick={handleAddCompany}
                >
                    Add New
                </button>
            </div>
        </>
    );
}

CompanyProfile.propTypes = {
    customerId: PropTypes.number.isRequired
};

export default CompanyProfile;