import { JwtService } from '@nestjs/jwt';
import { Repository } from 'typeorm';
import { User } from '../user/user.entity';
import { AuthCredentialsDto } from './dto/auth-credentials.dto';
export declare class AuthService {
    private userRepository;
    private jwtService;
    constructor(userRepository: Repository<User>, jwtService: JwtService);
    signUp(authCredentialsDto: AuthCredentialsDto): Promise<void>;
    validateUser(email: string, pass: string): Promise<any>;
    login(user: any): Promise<{
        access_token: any;
    }>;
}
