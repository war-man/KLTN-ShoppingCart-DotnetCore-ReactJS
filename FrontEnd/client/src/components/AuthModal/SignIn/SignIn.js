import React, { Component } from 'react';
import {Form, Input, Button, Checkbox } from 'antd';
import FormBuilder from 'antd-form-builder';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import {Link} from 'react-router-dom';
import {FaFacebookF, FaGoogle} from 'react-icons/fa';

 class SignIn extends Component {
    constructor(){
        super();
        this.onFinish=this.onFinish.bind(this);
       
        
    }
    //khai báo this form
    formLogin = FormBuilder.createForm(this)
    handleSignInNow(value){
        this.props.onSignInNowClick(value)
    }
    
    onFinish = (values) => {
        console.log('Received values of form: ',values);
        this.formLogin.resetFields();
    };
    
    
    
    render() {
        
        return (
            <>
                <Form
                    form={this.formLogin}
                    name="normal_login"
                    className="login-form"
                    initialValues={{
                        username: '',
                        password: '',
                        remember: false,
                    }}
                    onFinish={this.onFinish}
                    onFieldsChange={this.handleChangeLogin}
                    >
                    <Form.Item
                        name="username"
                        rules={[
                        {
                            required: true,
                            message: 'Xin vui lòng nhập tài khoản!',
                            
                        },
                        {
                            min: 10,
                            message: "Tài khoản phải có 8 ký tự trở lên!"
                        },
                        {
                            whitespace: true,
                            message: 'Tài khoản toàn là dấu cách!'
                        }
                        ]}
                        hasFeedback
                        
                        
                    >
                        <Input prefix={<UserOutlined
                        className="site-form-item-icon" />} 
                        
                        
                        placeholder="Username" 
                        
                        />
                    </Form.Item>
                    <Form.Item
                        
                        name="password"
                        rules={[
                        {
                            required: true,
                            message: 'Xin vui lòng nhập mật khẩu!',
                        },
                        {
                            min: 10,
                            message: "Mật khẩu phải có 8 ký tự trở lên!"
                        },
                        {
                            whitespace: true,
                            message: 'Mật khẩu toàn là dấu cách!'
                        }
                        ]}
                        hasFeedback
                        
                    >
                        <Input.Password
                        prefix={<LockOutlined className="site-form-item-icon" />}
                       
                        type="password"
                        
                        placeholder="Password"
                        />
                    </Form.Item>
                    <Form.Item>
                        <Form.Item name="remember" valuePropName="checked" noStyle>
                        <Checkbox>Nhớ tài khoản? </Checkbox>
                        </Form.Item>

                        <Link onClick={()=>this.handleSignInNow("3")} className="login-form-forgot" to="#">
                            Quên mật khẩu
                        </Link>
                    </Form.Item>

                    <Form.Item>
                        <Button type="primary" style={{height: '40px', border: '1px solid #fadb14', backgroundColor: '#fadb14'}} htmlType="submit" className="login-form-button">
                            Đăng nhập
                        </Button>
                    </Form.Item>
                    <div>
                        <Button type="primary" icon={<FaFacebookF></FaFacebookF>} style={{height: '40px', border: '1px solid #0050b3', backgroundColor: '#0050b3'}} className="login-form-button">
                            &nbsp;Đăng nhập bằng facebook
                        </Button>
                    </div>
                    <br></br>
                    <div>
                        <Button type="primary" icon={<FaGoogle></FaGoogle>} style={{height: '40px', border: '1px solid #f5222d', backgroundColor: '#f5222d'}} className="login-form-button">
                            &nbsp;Đăng nhập bằng google
                        </Button>
                    </div>
                </Form>
            </>
        )
    }
}

 export default SignIn;