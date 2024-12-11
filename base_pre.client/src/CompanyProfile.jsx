import { useEffect, useState } from 'react';
import PropTypes from 'prop-types';

function CompanyProfile({ customerId }) {
    const [profileData, setProfileData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [fadeIn, setFadeIn] = useState(false);

    useEffect(() => {
        async function fetchProfileData() {
            if (!customerId) return;

            try {
                setLoading(true);
                setFadeIn(false);
                const response = await fetch(`http://localhost:5000/api/ModelDbInits/GetCustomerByID/${customerId}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });

                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                const data = await response.json();
                setProfileData(data);
                setTimeout(() => {
                    setFadeIn(true);
                }, 100);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        }

        fetchProfileData();
    }, [customerId]);

    if (loading) {
        return (
            <div className="absolute inset-0 flex items-center justify-center bg-gray-50">
                <div className="text-center">
                    <div className="w-16 h-16 border-4 border-cyan-500 border-t-transparent rounded-full animate-spin mb-4"></div>
                    <p className="text-cyan-500 text-lg">Loading profile...</p>
                </div>
            </div>
        );
    }

    if (error) {
        return <div className="absolute p-4 text-red-500">Error loading profile: {error}</div>;
    }

    if (!profileData) {
        return <div className="absolute p-4 text-cyan-500">No profile data available.</div>;
    }

    const { clientInformation, modelDbInit, operationsStage1 } = profileData;

    return (
        <div className={`absolute inset-0 overflow-y-auto overflow-x-hidden bg-gray-50 transition-opacity duration-500 ease-in-out ${fadeIn ? 'opacity-100' : 'opacity-0'}`}>
            <div className="relative w-full min-h-full p-4">
                {/* Client Information Section */}
                <div className={`relative w-full bg-white rounded-lg shadow p-6 mb-8 transition-transform duration-500 ease-in-out ${fadeIn ? 'translate-y-0' : 'translate-y-4'}`}>
                    <h2 className="text-xl font-bold text-cyan-600 mb-4">Company Profile</h2>
                    <div className="space-y-2">
                        <p className="text-gray-600">Name: {clientInformation?.clientFirstName} {clientInformation?.clientLastName}</p>
                        <p className="text-gray-600">Company: {clientInformation?.companyName}</p>
                        <p className="text-gray-600">Phone: {clientInformation?.cleintPhone}</p>
                        <p className="text-gray-600">Address: {clientInformation?.clientAddress}</p>
                        <p className="text-gray-600">Customer ID: {clientInformation?.customerId}</p>
                        <p className="text-gray-600">Client ID: {clientInformation?.id}</p>
                    </div>
                </div>

                {/* Model Info Section */}
                {modelDbInit && (
                    <div className={`relative w-full bg-white rounded-lg shadow p-6 mb-8 transition-transform duration-500 delay-100 ease-in-out ${fadeIn ? 'translate-y-0' : 'translate-y-4'}`}>
                        <h3 className="text-lg font-bold text-cyan-600 mb-4">Model Information</h3>
                        <div className="space-y-2">
                            <p className="text-gray-600">Model ID: {modelDbInit.modelId}</p>
                            <p className="text-gray-600">Timestamp: {new Date(modelDbInit.modelDbInitTimeStamp).toLocaleString()}</p>
                            <p className="text-gray-600">Data Size: {modelDbInit.dataSize} bytes</p>
                            <p className="text-gray-600">Product Vector: {modelDbInit.modelDbInitProductVector}</p>
                            <p className="text-gray-600">Service Vector: {modelDbInit.modelDbInitServiceVector || 'N/A'}</p>
                        </div>
                    </div>
                )}

                {/* Operations Section */}
                {operationsStage1 && (
                    <div className={`relative w-full bg-white rounded-lg shadow p-6 mb-8 transition-transform duration-500 delay-200 ease-in-out ${fadeIn ? 'translate-y-0' : 'translate-y-4'}`}>
                        <h3 className="text-lg font-bold text-cyan-600 mb-4">Operations Information</h3>

                        {/* Products Section */}
                        <div className="mb-6">
                            <h4 className="text-md font-semibold text-cyan-500 mb-4">Products</h4>
                            <div className="space-y-4">
                                {['A', 'B', 'C'].map((letter, index) => (
                                    <div key={letter} className={`border p-4 rounded transition-transform duration-500 delay-${300 + index * 100} ease-in-out ${fadeIn ? 'translate-y-0' : 'translate-y-4'}`}>
                                        <p className="font-medium">Product {letter}</p>
                                        <p className="text-sm text-gray-600">Name: {operationsStage1[`product${letter}`]?.productName}</p>
                                        <p className="text-sm text-gray-600">Type: {operationsStage1[`product${letter}`]?.productType}</p>
                                    </div>
                                ))}
                            </div>
                        </div>

                        {/* Services Section */}
                        <div>
                            <h4 className="text-md font-semibold text-cyan-500 mb-4">Services</h4>
                            <div className="space-y-4">
                                {['A', 'B', 'C'].map((letter, index) => (
                                    <div key={letter} className={`border p-4 rounded transition-transform duration-500 delay-${600 + index * 100} ease-in-out ${fadeIn ? 'translate-y-0' : 'translate-y-4'}`}>
                                        <p className="font-medium">Service {letter}</p>
                                        <p className="text-sm text-gray-600">Name: {operationsStage1[`service${letter}`]?.serviceName}</p>
                                        <p className="text-sm text-gray-600">Type: {operationsStage1[`service${letter}`]?.serviceType}</p>
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}

CompanyProfile.propTypes = {
    customerId: PropTypes.number.isRequired
};

export default CompanyProfile;