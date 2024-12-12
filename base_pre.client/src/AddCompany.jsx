import PropTypes from 'prop-types';

function AddCompany({ onClose }) {
    const modalStyles = {
        position: 'fixed',
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        background: 'rgba(0, 0, 0, 0.85)',
        backdropFilter: 'blur(10px)',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        zIndex: 11000
    };

    const contentStyles = {
        background: 'rgba(0, 0, 0, 0.95)',
        padding: '30px',
        borderRadius: '8px',
        width: '90%',
        maxWidth: '600px',
        border: '1px solid rgba(87, 179, 192, 0.3)',
        color: '#57b3c0',
        position: 'relative',
        boxShadow: '0 0 30px rgba(87, 179, 192, 0.2)'
    };

    const closeButtonStyles = {
        position: 'absolute',
        top: '15px',
        right: '15px',
        background: 'none',
        border: 'none',
        color: '#57b3c0',
        fontSize: '24px',
        cursor: 'pointer',
        padding: '5px 10px',
        borderRadius: '4px',
        transition: 'all 0.3s ease'
    };

    return (
        <div style={modalStyles}>
            <div style={contentStyles}>
                <button
                    style={closeButtonStyles}
                    onClick={onClose}
                >
                    ×
                </button>
                <h2 style={{
                    fontSize: '24px',
                    marginBottom: '20px',
                    textAlign: 'center'
                }}>
                    Add New Company
                </h2>
                {/* Add your form components here */}
            </div>
        </div>
    );
}

AddCompany.propTypes = {
    onClose: PropTypes.func.isRequired
};

export default AddCompany;
